using System;
using System.Collections.ObjectModel;
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
using FlameBase.RenderMachine.Models;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.Providers;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.ViewModels
{
    internal class PostFlameViewModel : Notifier
    {
        private readonly PostModel _postModel = new PostModel();

        private ColorPickProvider _currentColorPickProvider;
        private GradientPickProvider _currentGradientPickProvider;

        private BitmapSource _currentImg;
        private LoaderSaverProvider _currentLoaderSaverProvider;

        private string _currentMultiCommand;
        private int _currentSelectedColorIndex = -1;
        private LogDisplayModel _displayModel;


        public PostFlameViewModel()
        {
            MultiCommand = new RelayCommand(MultiCommandHandler);
            CommandRadioChecked = new RelayCommand(RadioCheckedHandler);
            CommandBackColor = new RelayCommand(BackColorHandler);
            CommandSetGradient = new RelayCommand(SetGradientHandler);
            RadioColor = true;
            GradientVisibility = Visibility.Collapsed;
        }

        private void SelectColorForGradient(Color color)
        {
            _currentSelectedColorIndex = -2;
            _currentColorPickProvider = new ColorPickProvider(ColorPickProviderCallback, color);
            _currentColorPickProvider.Exec();
        }

        #region events

        private void RectangleMouseDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = (Rectangle) sender;
            _currentSelectedColorIndex = ColorRectangles.IndexOf(rectangle);

            if (RadioColor)
            {
                var color = _currentLoaderSaverProvider.Flame.FunctionColors[_currentSelectedColorIndex];
                _currentColorPickProvider = new ColorPickProvider(ColorPickProviderCallback, color);
                _currentColorPickProvider.Exec();
            }

            else if (RadioGradient)
            {
                var color = _currentLoaderSaverProvider.Flame.GradientPack.Values[_currentSelectedColorIndex];
                _currentGradientPickProvider =
                    new GradientPickProvider(GradientPickProviderCallback, _postModel.GradientModel, color);
                _currentGradientPickProvider.Exec();
            }
        }

        #endregion

        #region set ui

        private void SetUi()
        {
            var brushBorder = (Brush) Application.Current.Resources["BrushBorder"];
            if (RadioGradient == false)
            {
                ClearRectangles();
                foreach (var color in _postModel.TransformColors)
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
                GradientFill = new LinearGradientBrush(_postModel.GradientModel.GetGradientStopCollection());
                foreach (var gradientValue in _postModel.GradientValues)
                {
                    var color = gradientValue;
                    var rectangle = new Rectangle
                    {
                        Width = 30,
                        Height = 28,
                        Fill = new SolidColorBrush(_postModel.GradientModel.GetFromPosition(color)),
                        Stroke = brushBorder,
                        Margin = new Thickness(3)
                    };
                    rectangle.MouseDown += RectangleMouseDown;
                    ColorRectangles.Add(rectangle);
                }
            }

            BackColor = new SolidColorBrush(_postModel.BackColor);
        }

        private void SetColor(Color color)
        {
            _postModel.TransformColors[_currentSelectedColorIndex] = color;
            _currentSelectedColorIndex = -1;
        }

        private void SetBackColor(Color color)
        {
            _postModel.BackColor = color;
            _displayModel.BackColor = color;
            _currentSelectedColorIndex = -1;
        }

        private void ClearRectangles()
        {
            foreach (var rectangle in ColorRectangles) rectangle.MouseDown -= RectangleMouseDown;
            ColorRectangles.Clear();
        }

        private void InitPost()
        {
            if (!_currentLoaderSaverProvider.Result) return;

            var flameModel = _currentLoaderSaverProvider.Flame;
            GetGradientModel(flameModel, out var gradientModel, out var gradientValues);
            if (gradientModel != null)
            {
                _postModel.GradientModel = gradientModel;
                _postModel.GradientValues = gradientValues;
                RadioGradient = true;
            }

            _postModel.BackColor = flameModel.BackColor;
            _postModel.TransformColors = flameModel.FunctionColors.ToArray();
            _postModel.DisplayArray = _currentLoaderSaverProvider.DisplayArray;
            _displayModel = new LogDisplayModel(_postModel.DisplayArray, _postModel.BackColor);
        }

        private async void ShowRender()
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ImageSource = null;
                    ImageTopContent = StaticClasses.GetSpinner();
                });
            });

            await Task.Run(() =>
            {
                if (RadioColor) _currentImg = _displayModel.GetBitmapForRender(_postModel.TransformColors);

                if (RadioGradient)
                    _currentImg = _displayModel.GetBitmapForRender(_postModel.GradientValues, _postModel.GradientModel);

                ImageSource = _currentImg;
                ImageTopContent = null;
                Application.Current.Dispatcher.Invoke(SetUi);
            });
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
            _currentColorPickProvider = new ColorPickProvider(ColorPickProviderCallback, color);
        }

        private void RadioCheckedHandler(object obj)
        {
            if (RadioColor) GradientVisibility = Visibility.Collapsed;

            if (RadioGradient) GradientVisibility = Visibility.Visible;
        }

        private void SetGradientHandler(object obj)
        {
            _currentGradientPickProvider =
                new GradientPickProvider(GradientPickProviderCallback, _postModel.GradientModel);
            _currentGradientPickProvider.Exec();
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
                    _currentLoaderSaverProvider.Exec();
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
                var filename = GetImageName(_currentLoaderSaverProvider.FlameName);
                ImageHelper.SaveImage(filename, _currentImg);
            });
            TopContent = null;
        }

        private static string GetImageName(string flameName)
        {
            var now = DateTime.Now;
            return
                $"{Directories.Images}\\{flameName}_{now.Year}{now.Month:00}{now.Day:00}-{(int) now.TimeOfDay.TotalSeconds}.png";
        }

        #endregion

        #region providers

        private void LoaderSaverProviderCallback(ProviderEnums.CallbackType callbackType, string message)
        {
            switch (callbackType)
            {
                case ProviderEnums.CallbackType.ShowControl:
                    TopContent = _currentLoaderSaverProvider.ShowControl;
                    return;
                case ProviderEnums.CallbackType.ShowSpinner:
                    Application.Current.Dispatcher.Invoke(() => { TopContent = StaticClasses.GetSpinner(message); });
                    return;
                case ProviderEnums.CallbackType.End:
                    switch (_currentMultiCommand)
                    {
                        case "loadRender":
                            InitPost();
                            ShowRender();
                            break;
                    }

                    _currentMultiCommand = string.Empty;
                    ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_BOTTOM_STRING",
                        _currentLoaderSaverProvider.ResultString);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }

        private void ColorPickProviderCallback(ProviderEnums.CallbackType callbackType, string message)
        {
            switch (callbackType)
            {
                case ProviderEnums.CallbackType.ShowControl:
                    TopContent = _currentColorPickProvider.ShowControl;
                    return;
                case ProviderEnums.CallbackType.ShowSpinner:
                    TopContent = StaticClasses.GetSpinner(message);
                    return;
                case ProviderEnums.CallbackType.End:
                    TopContent = null;
                    if (!_currentColorPickProvider.Result)
                    {
                        _currentColorPickProvider = null;
                        return;
                    }

                    if (_currentSelectedColorIndex == -1)
                        SetBackColor(_currentColorPickProvider.ResultColor);
                    else if (_currentSelectedColorIndex == -2)
                    {
                        var resultColor = _currentColorPickProvider.ResultColor;
                        _postModel.GradientModel.ChangeColor(1, resultColor);
                        _currentColorPickProvider = null;
                        _currentGradientPickProvider =
                            new GradientPickProvider(GradientPickProviderCallback, _postModel.GradientModel);
                        _currentGradientPickProvider.Exec();
                    }
                    else
                        SetColor(_currentColorPickProvider.ResultColor);

                    _currentColorPickProvider = null;
                    ShowRender();
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }

        private void GradientPickProviderCallback(ProviderEnums.CallbackType callbackType, string message)
        {
            switch (callbackType)
            {
                case ProviderEnums.CallbackType.ShowControl:
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        TopContent = _currentGradientPickProvider.ShowControl;
                        break;
                    }

                    TopContent = null;
                    var color = (Color) ColorConverter.ConvertFromString("#FFDFD991");
                    SelectColorForGradient(color);

                    break;
                case ProviderEnums.CallbackType.ShowSpinner:
                    TopContent = StaticClasses.GetSpinner(message);
                    break;
                case ProviderEnums.CallbackType.End:
                    TopContent = null;
                    if (!_currentGradientPickProvider.Result) return;
                    switch (_currentGradientPickProvider.PickerType)
                    {
                        case GradientPickProvider.GradientPickerType.Gradient:
                            _postModel.GradientModel = _currentGradientPickProvider.ResultGradientModel;
                            break;
                        case GradientPickProvider.GradientPickerType.Position:
                            _postModel.GradientValues[_currentSelectedColorIndex] =
                                _currentGradientPickProvider.ResultGradientPosition;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    _currentGradientPickProvider = null;
                    _currentSelectedColorIndex = -1;
                    ShowRender();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
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