using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FlameBase.Enums;
using FlameBase.Helpers;
using FlameBase.Models;
using FlameBase.RenderMachine;
using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.ViewModels
{
    internal class PostFlameViewModel : Notifier
    {
        private ColorPickProvider _colorPickProvider;
        private LoaderSaverProvider _loaderSaverProvider;
        private string _multiCommand;
        private int _selectedColorIndex = -1;
        private BitmapSource _img;

        public PostFlameViewModel()
        {
            MultiCommand = new RelayCommand(MultiCommandHandler);
            CommandRadioChecked = new RelayCommand(RadioCheckedHandler);
            CommandBackColor = new RelayCommand(BackColorHandler);
            RadioColor = true;
            GradientVisibility = Visibility.Collapsed;
        }


        private static void GetGradientModel(FlameModel flameModel, out GradientModel gradientModel,
            out double[] gradientValues)
        {
            gradientModel = null;
            gradientValues = null;
            if (flameModel?.GradientPack == null) return;
            var gm = new GradientModel(flameModel.GradientPack);
            gradientModel = gm.Copy();
            gradientValues = flameModel.FunctionColorPositions.ToArray();
        }

        #region events

        private void RectangleMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            var rectangle = (Rectangle) sender;
            _selectedColorIndex = ColorRectangles.IndexOf(rectangle);

            if (RadioColor)
            {
                var color = _loaderSaverProvider.Flame.FunctionColors[_selectedColorIndex];
                Debug.WriteLine($"{_selectedColorIndex} {color}");
                _colorPickProvider = new ColorPickProvider(ColorPickerProviderCallback, color);
            }

            if (RadioGradient)
            {
                var color = _loaderSaverProvider.Flame.GradientPack.Values[_selectedColorIndex];
                //_gradientPickProvider = new GradientPickProvider()
            }
        }

        #endregion


        #region set ui

        private async void ShowRender(FlameModel flameModel)
        {
            if (flameModel == null) return;
            GetGradientModel(flameModel, out var gradientModel, out var gradientValues);


            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ImageSource = null;
                    ImageTopContent = new SpinnerView();
                });
                Thread.Sleep(100);
            });

            await Task.Run(() =>
            {
                Thread.Sleep(100);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _img = RenderMachine.GetImage(
                        flameModel.FunctionColors.ToArray(), gradientValues,
                        gradientModel);
                    ImageSource = _img;
                    ImageTopContent = null;

                    SetUi(flameModel, gradientModel, gradientValues);
                });
            });
        }

        private void SetUi(FlameModel flameModel, GradientModel gradientModel, double[] gradientValues)
        {
            var brushBorder = (Brush) Application.Current.Resources["BrushBorder"];
            if (gradientModel == null)
            {
                RadioColor = true;
                ClearRectangles();
                foreach (var color in flameModel.FunctionColors)
                {
                    var rectangle = new Rectangle
                    {
                        Width = 30, Height = 28, Fill = new SolidColorBrush(color),
                        Stroke = brushBorder,
                        Margin = new Thickness(3)
                    };
                    rectangle.MouseDown += RectangleMouseDown;
                    ColorRectangles.Add(rectangle);
                }
            }
            else
            {
                ColorRectangles.Clear();
                RadioGradient = true;
                GradientFill = new LinearGradientBrush(gradientModel.GetGradientStopCollection());
                foreach (var gradientValue in gradientValues)
                {
                    var color = gradientValue;
                    var rectangle = new Rectangle
                    {
                        Width = 30,
                        Height = 28,
                        Fill = new SolidColorBrush(gradientModel.GetFromPosition(color)),
                        Stroke = brushBorder,
                        Margin = new Thickness(3)
                    };
                    rectangle.MouseDown += RectangleMouseDown;
                    ColorRectangles.Add(rectangle);
                }
            }

            BackColor = new SolidColorBrush(flameModel.BackColor);
        }

        private void SetColor(Color color)
        {
            _loaderSaverProvider.Flame.FunctionColors[_selectedColorIndex] = color;
            ShowRender(_loaderSaverProvider);
            _selectedColorIndex = -1;
        }

        private void SetBackColor(Color color)
        {
            _loaderSaverProvider.Flame.BackColor = color;
            RenderMachine.Display.BackColor = color;
            ShowRender(_loaderSaverProvider);
            _selectedColorIndex = -1;
        }


        private void ClearRectangles()
        {
            foreach (var rectangle in ColorRectangles) rectangle.MouseDown -= RectangleMouseDown;
            ColorRectangles.Clear();
        }

        private void ShowRender(LoaderSaverProvider loaderSaverProvider)
        {
            if (!loaderSaverProvider.Result) return;
            var flameModel = loaderSaverProvider.Flame;
            ShowRender(flameModel);
        }

        #endregion

        #region handlers

        private void BackColorHandler(object obj)
        {
            var color = BackColor.Color;
            _colorPickProvider = new ColorPickProvider(ColorPickerProviderCallback, color);
        }

        private void RadioCheckedHandler(object obj)
        {
            if (RadioColor) GradientVisibility = Visibility.Collapsed;

            if (RadioGradient) GradientVisibility = Visibility.Visible;
        }

        private void MultiCommandHandler(object obj)
        {
            if (!(obj is string mc)) return;
            _multiCommand = mc;

            switch (mc)
            {
                case "toCreateFlame":
                    ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_WINDOW_CONTENT_BY_PARAMS", "CreateFlame", null);
                    return;
                case "loadRender":
                    _loaderSaverProvider =
                        new LoaderSaverProvider(FileViewType.LoadRender, LoaderSaverProviderCallback);
                    return;
                case "saveRender":
                    return;
                case "saveImage":
                    if (_img == null) return;
                    SaveImage();
                    return;
            }

            _multiCommand = string.Empty;
        }

        private async void SaveImage()
        {
            StaticClasses.SetSpinnerText("saving image...");
            TopContent = StaticClasses.Spinner;

            await Task.Run(() =>
            {
                var now = DateTime.Now;
                var filename =
                    $"{Directories.Images}\\{_loaderSaverProvider.FlameName}_{now.Year}{now.Month:00}{now.Day:00}-{(int) now.TimeOfDay.TotalSeconds}.png";
                ImageHelper.SaveImage(filename, _img);
            });
            TopContent = null;
        }

        #endregion

        #region providers

        private void LoaderSaverProviderCallback(string command, string message, Control control)
        {
            if (command == "spinner")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var spinner = new SpinnerView();
                    var vm = (SpinnerViewModel) spinner.DataContext;
                    vm.Text = message;
                    TopContent = spinner;
                });
                return;
            }

            TopContent = control;
            if (control != null) return;


            switch (_multiCommand)
            {
                case "loadRender":
                    ShowRender(_loaderSaverProvider);
                    break;
            }

            _multiCommand = string.Empty;
            ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_BOTTOM_STRING", _loaderSaverProvider.ResultString);
        }

        private void ColorPickerProviderCallback(string command, string message, Control control)
        {
            switch (command)
            {
                case "picker-view":
                    TopContent = control;
                    break;
                case "end":
                    TopContent = null;
                    if (!_colorPickProvider.Result) break;

                    if (_selectedColorIndex == -1)
                        SetBackColor(_colorPickProvider.ResultColor);
                    else
                        SetColor(_colorPickProvider.ResultColor);

                    break;
            }
        }

        #endregion

        #region bindings

        public ICommand MultiCommand { get; }

        [ValueBind]
        public Control TopContent
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public BitmapSource ImageSource
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Control ImageTopContent
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool RadioColor
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool RadioGradient
        {
            get => Get();
            set => Set(value);
        }

        public ICommand CommandRadioChecked { get; }
        public ICommand CommandBackColor { get; }

        [ValueBind]
        public Visibility GradientVisibility
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Brush GradientFill
        {
            get => Get();
            set => Set(value);
        }

        public ObservableCollection<Rectangle> ColorRectangles { get; set; } = new ObservableCollection<Rectangle>();

        [ValueBind]
        public SolidColorBrush BackColor
        {
            get => Get();
            set => Set(value);
        }

        #endregion
    }
}