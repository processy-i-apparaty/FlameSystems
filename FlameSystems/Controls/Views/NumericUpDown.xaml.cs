using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace FlameSystems.Controls.Views
{
    /// <summary>
    ///     Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown
    {
        private static Brush _brushForeground;


        public NumericUpDown()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _brushForeground = Resources["BrushForeground"] as SolidColorBrush;
        }

        #region methods
        // private static Size MeasureString(string candidate, TextBlock textBlock)
        // {
        //     var formattedText = new FormattedText(
        //         candidate,
        //         CultureInfo.CurrentCulture,
        //         FlowDirection.LeftToRight,
        //         new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
        //         textBlock.FontSize,
        //         _brushForeground,
        //         new NumberSubstitution(),
        //         1);
        //     return new Size(formattedText.Width, formattedText.Height);
        // }

        private void SetValueFromInput()
        {
            var input = TextNumbers.Text;
            input = input.Replace(',', '.');
            if (!double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var val))
            {
                TextNumbers.Text = Value.ToString(CultureInfo.InvariantCulture);
                return;
            }

            Value = val;
            SelectAllNumbers();
        }

        private void SelectAllNumbers()
        {
            var length = TextNumbers.Text.Length;
            TextNumbers.SelectionStart = 0;
            TextNumbers.SelectionLength = length;
        }

        private void Increase(double n)
        {
            var nextValue = Value + n;
            Value = nextValue;
        }

        private static bool CheckInput(ref char c, ref string text)
        {
            switch (c)
            {
                case ',':
                case '.':
                    return text.IndexOf(',') + text.IndexOf('.') >= -1;
                case '-':
                    if (text.Length > 0) return true;
                    return text.IndexOf('-') > -1;
            }

            return false;
        }

        private void ChangeValue()
        {
            // Debug.WriteLine($"ChangeValue {Value}");

            var format = $"{{0:N{DecimalPlaces}}}";
            var text = string.Format(format, Value).Replace(',', '.');
            TextNumbers.Text = text;
        }

        private void CheckValue()
        {
            var v = Value;
            if (v < Minimum) v = Minimum;
            if (v > Maximum) v = Maximum;
            v = Math.Round(v, DecimalPlaces);
            var format = $"{{0:N{DecimalPlaces}}}";
            var text = string.Format(format, v).Replace(',', '.');
            if (TextNumbers.Text == text) return;
            TextNumbers.Text = text;
            Value = v;
        }

        #endregion

        #region events

        private void TextNumbersOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = TextNumbers.Text;

            if (txt.Length == 0) return;
            var numberOnly = Regex.Replace(txt, "[^0-9.,-]", "");
            var lastChar = numberOnly.Last();
            var numberOnlyWithoutLast = numberOnly.Substring(0, numberOnly.Length - 1);

            if (CheckInput(ref lastChar, ref numberOnlyWithoutLast))
            {
                TextNumbers.Text = numberOnlyWithoutLast;
                return;
            }

            TextNumbers.Text = numberOnly;
            TextNumbers.CaretIndex = numberOnly.Length;
        }


        private void ButtonDown_OnClick(object sender, RoutedEventArgs e)
        {
            Increase(-ChangeNormal);
        }

        private void ButtonUp_OnClick(object sender, RoutedEventArgs e)
        {
            Increase(ChangeNormal);
        }

        private static void OnDependencyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var numericUpDown = dependencyObject as NumericUpDown;
            var property = e.Property;
            numericUpDown?.OnDependencyChanged(property, e);
        }

        private void OnDependencyChanged(DependencyProperty property, DependencyPropertyChangedEventArgs e)
        {
            var val = e.NewValue;
            if (property == ValueProperty)
            {
                ChangeValue();
            }
            else if (property == DecimalPlacesProperty)
            {
                Debug.WriteLine($"DecimalPlacesProperty {val} {DecimalPlaces}");
                if ((int)val < 0)
                {
                    DecimalPlaces = 0;
                    return;
                }

                CheckValue();
            }
            else if (property == MaximumProperty)
            {
                Debug.WriteLine($"MaximumProperty {val}");
                CheckValue();
            }
            else if (property == MinimumProperty)
            {
                Debug.WriteLine($"MinimumProperty {val}");
                CheckValue();
            }
            else if (property == ChangeNormalProperty)
            {
                Debug.WriteLine($"ChangeNormalProperty {val}");
            }
            else if (property == ChangeLargeProperty)
            {
                Debug.WriteLine($"ChangeLargeProperty {val}");
            }
            else if (property == ChangeSmallProperty)
            {
                Debug.WriteLine($"ChangeSmallProperty {val}");
            }
            else if (property == ChangeTinyProperty)
            {
                Debug.WriteLine($"ChangeTinyProperty {val}");
            }
        }


        private void ScannerOnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //var position = e.GetPosition(TextNumbers);
            var change = GetChangeValue();

            if (e.Delta > 0) Increase(change);
            else if (e.Delta < 0) Increase(-change);

            if (Keyboard.FocusedElement is Window)
            {
                Debug.WriteLine(Keyboard.FocusedElement);
                // Keyboard.FocusedElement.f
                Debug.WriteLine($"{TextNumbers.Focus()} -");
            }

            // Debug.WriteLine($"{Scanner.Focus()} -");
        }

        private void ScannerOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextNumbers.Focus();
            SelectAllNumbers();
        }

        private double GetChangeValue()
        {
            var change = ChangeNormal;
            switch (Keyboard.Modifiers)
            {
                case ModifierKeys.Control:
                    change = ChangeSmall;
                    break;
                case ModifierKeys.Shift:
                    change = ChangeLarge;
                    break;
                case ModifierKeys.Alt:
                    change = ChangeTiny;
                    // Focus();
                    break;
            }

            return change;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var change = GetChangeValue();
            switch (e.Key)
            {
                case Key.Down:
                    Increase(-change);
                    break;
                case Key.Up:
                    Increase(change);
                    break;
                case Key.Enter:
                    SetValueFromInput();
                    break;
            }
        }

        #endregion

        #region dependency properties

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new
                FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                DefaultValue = 0.0,
                PropertyChangedCallback = OnDependencyChanged
            });

        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(NumericUpDown),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    DefaultValue = 0,
                    PropertyChangedCallback = OnDependencyChanged

                });


        public static readonly DependencyProperty ChangeNormalProperty =
            DependencyProperty.Register("ChangeNormal", typeof(double), typeof(NumericUpDown), new
                FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                DefaultValue = 1.0,
                PropertyChangedCallback = OnDependencyChanged
            });


        public static readonly DependencyProperty ChangeSmallProperty =
            DependencyProperty.Register("ChangeSmall", typeof(double), typeof(NumericUpDown), new
                FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                DefaultValue = 0.1,
                PropertyChangedCallback = OnDependencyChanged
            });


        public static readonly DependencyProperty ChangeLargeProperty =
            DependencyProperty.Register("ChangeLarge", typeof(double), typeof(NumericUpDown), new
                FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                DefaultValue = 10.0,
                PropertyChangedCallback = OnDependencyChanged
            });


        public static readonly DependencyProperty ChangeTinyProperty =
            DependencyProperty.Register("ChangeTiny", typeof(double), typeof(NumericUpDown), new
                FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                DefaultValue = 0.01,
                PropertyChangedCallback = OnDependencyChanged
            });


        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(NumericUpDown), new
                FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                DefaultValue = double.MinValue,
                PropertyChangedCallback = OnDependencyChanged
            });


        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(NumericUpDown), new
                FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                DefaultValue = double.MaxValue,
                PropertyChangedCallback = OnDependencyChanged
            });


        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }


        public double ChangeNormal
        {
            get => (double)GetValue(ChangeNormalProperty);
            set => SetValue(ChangeNormalProperty, value);
        }

        public double ChangeSmall
        {
            get => (double)GetValue(ChangeSmallProperty);
            set => SetValue(ChangeSmallProperty, value);
        }

        public double ChangeLarge
        {
            get => (double)GetValue(ChangeLargeProperty);
            set => SetValue(ChangeLargeProperty, value);
        }

        public double ChangeTiny
        {
            get => (double)GetValue(ChangeTinyProperty);
            set => SetValue(ChangeTinyProperty, value);
        }


        public int DecimalPlaces
        {
            get => (int)GetValue(DecimalPlacesProperty);
            set
            {
                var v = value;
                if (v < 0) v = 0;
                SetValue(DecimalPlacesProperty, v);
            }
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set
            {
                var d = Math.Round(value, DecimalPlaces);
                if (d > Maximum) d = Maximum;
                if (d < Minimum) d = Minimum;
                SetValue(ValueProperty, d);
                ChangeValue();
            }
        }

        #endregion
    }
}