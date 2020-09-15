using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.ViewModels
{
    internal class PostFlameViewModel : Notifier
    {
        private ColorPickProvider _currentColorPickProvider;
        private BitmapSource _currentImg;
        private LoaderSaverProvider _currentLoaderSaverProvider;
        private string _currentMultiCommand;
        private int _currentSelectedColorIndex = -1;

        public PostFlameViewModel()
        {
            MultiCommand = new RelayCommand(MultiCommandHandler);
            CommandRadioChecked = new RelayCommand(RadioCheckedHandler);
            CommandBackColor = new RelayCommand(BackColorHandler);
            CommandSetGradient = new RelayCommand(SetGradientHandler);
            RadioColor = true;
            GradientVisibility = Visibility.Collapsed;
        }

        #region events

        private void RectangleMouseDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = (Rectangle) sender;
            _currentSelectedColorIndex = ColorRectangles.IndexOf(rectangle);

            if (RadioColor)
            {
                var color = _currentLoaderSaverProvider.Flame.FunctionColors[_currentSelectedColorIndex];
                Debug.WriteLine($"{_currentSelectedColorIndex} {color}");
                _currentColorPickProvider = new ColorPickProvider(ColorPickerProviderCallback, color);
            }

            if (RadioGradient)
            {
                var color = _currentLoaderSaverProvider.Flame.GradientPack.Values[_currentSelectedColorIndex];
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
                    ImageTopContent = StaticClasses.GetSpinner();
                });
                // Thread.Sleep(100);
            });

            await Task.Run(() =>
            {
                _currentImg = RenderMachine.GetImage(
                    flameModel.FunctionColors.ToArray(), gradientValues,
                    gradientModel);
                ImageSource = _currentImg;
                ImageTopContent = null;
                Application.Current.Dispatcher.Invoke(() => { SetUi(flameModel, gradientModel, gradientValues); });
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
            _currentLoaderSaverProvider.Flame.FunctionColors[_currentSelectedColorIndex] = color;
            ShowRender(_currentLoaderSaverProvider);
            _currentSelectedColorIndex = -1;
        }

        private void SetBackColor(Color color)
        {
            _currentLoaderSaverProvider.Flame.BackColor = color;
            RenderMachine.Display.BackColor = color;
            ShowRender(_currentLoaderSaverProvider);
            _currentSelectedColorIndex = -1;
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

        #endregion

        #region handlers

        private void BackColorHandler(object obj)
        {
            var color = BackColor.Color;
            _currentColorPickProvider = new ColorPickProvider(ColorPickerProviderCallback, color);
        }

        private void RadioCheckedHandler(object obj)
        {
            if (RadioColor) GradientVisibility = Visibility.Collapsed;

            if (RadioGradient) GradientVisibility = Visibility.Visible;
        }

        private void SetGradientHandler(object obj)
        {
        }

        private void MultiCommandHandler(object obj)
        {
            if (!(obj is string mc)) return;
            _currentMultiCommand = mc;

            switch (mc)
            {
                case "toCreateFlame":
                    RenderMachine.DestroyDisplay();
                    ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_WINDOW_CONTENT_BY_PARAMS", "CreateFlame", null);
                    return;
                case "loadRender":
                    _currentLoaderSaverProvider =
                        new LoaderSaverProvider(FileViewType.LoadRender, LoaderSaverProviderCallback);
                    return;
                case "saveRender":
                    return;
                case "saveImage":
                    SaveImage();
                    return;
            }

            _currentMultiCommand = string.Empty;
        }

        private async void SaveImage()
        {
            if (_currentImg == null) return;

            TopContent = StaticClasses.GetSpinner();

            await Task.Run(() =>
            {
                var now = DateTime.Now;
                var filename =
                    $"{Directories.Images}\\{_currentLoaderSaverProvider.FlameName}_{now.Year}{now.Month:00}{now.Day:00}-{(int) now.TimeOfDay.TotalSeconds}.png";
                ImageHelper.SaveImage(filename, _currentImg);
            });
            TopContent = null;
        }

        #endregion

        #region providers

        private void LoaderSaverProviderCallback(string command, string message, Control control)
        {
            if (command == "spinner")
            {
                Application.Current.Dispatcher.Invoke(() => { TopContent = StaticClasses.GetSpinner(message); });
                return;
            }

            TopContent = control;
            if (control != null) return;


            switch (_currentMultiCommand)
            {
                case "loadRender":
                    ShowRender(_currentLoaderSaverProvider);
                    break;
            }

            _currentMultiCommand = string.Empty;
            ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_BOTTOM_STRING", _currentLoaderSaverProvider.ResultString);
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
                    if (!_currentColorPickProvider.Result) break;

                    if (_currentSelectedColorIndex == -1)
                        SetBackColor(_currentColorPickProvider.ResultColor);
                    else
                        SetColor(_currentColorPickProvider.ResultColor);
                    _currentColorPickProvider = null;

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

        public ICommand CommandSetGradient { get; }

        #endregion
    }
}