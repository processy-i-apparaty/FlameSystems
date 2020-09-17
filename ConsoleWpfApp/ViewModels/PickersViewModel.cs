using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ConsoleWpfApp.Infrastructure;
using ConsoleWpfApp.Infrastructure.ValueBind;
using ConsoleWpfApp.Pickers.Enums;
using ConsoleWpfApp.Pickers.Models;
using ConsoleWpfApp.Pickers.Providers;
using ConsoleWpfApp.Pickers.Views;

namespace ConsoleWpfApp.ViewModels
{
    internal class PickersViewModel : Notifier
    {
        private GradientModel _gradientModel;
        private Color _color;
        private double _gradientValue;
        private ColorPickProvider _colorPickProvider;
        private GradientPickProvider _gradientPickProvider;

        public PickersViewModel()
        {
            _gradientModel = new GradientModel(Colors.CornflowerBlue, Colors.Crimson, new[] {Colors.OrangeRed},
                new[] {0.5});
            _color = Colors.BlueViolet;
            _gradientValue = .33;
            CommandMouseDown = new RelayCommand(HandlerMouseDown);
            UpdateUi();
        }

        private void UpdateUi()
        {
            BrushColor = new SolidColorBrush(_color);
            BrushGradientValue = new SolidColorBrush(_gradientModel.GetFromPosition(_gradientValue));
            BrushGradient = new LinearGradientBrush(_gradientModel.GetGradientStopCollection(),0.0);
        }

        #region handlers

        #endregion

        private void HandlerMouseDown(object obj)
        {
            Debug.WriteLine(obj);
            switch ((string) obj)
            {
                case "color":
                    _colorPickProvider = new ColorPickProvider(CallbackColorPickProvider, _color);
                    _colorPickProvider.Exec();
                    break;
                case "gradient-value":
                    _gradientPickProvider =
                        new GradientPickProvider(CallbackGradientPickProvider, _gradientModel, _gradientValue);
                    _gradientPickProvider.Exec();
                    break;
                case "gradient":
                    _gradientPickProvider =
                        new GradientPickProvider(CallbackGradientPickProvider, _gradientModel);
                    _gradientPickProvider.Exec();
                    break;
            }
        }

        private void CallbackGradientPickProvider(ProviderCallbackType callbackType, string message)
        {
            switch (callbackType)
            {
                case ProviderCallbackType.ShowControl:
                    TopContent = _gradientPickProvider.ShowControl;
                    break;
                case ProviderCallbackType.End:
                    TopContent = null;
                    if (_gradientPickProvider.Result)
                        switch (_gradientPickProvider.GradientMode)
                        {
                            case GradientMode.Edit:
                                _gradientModel = _gradientPickProvider.ResultGradientModel.Copy();
                                break;
                            case GradientMode.Select:
                                _gradientValue = _gradientPickProvider.ResultValue;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                    if (_gradientPickProvider.Result) UpdateUi();
                    _gradientPickProvider = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }

        private void CallbackColorPickProvider(ProviderCallbackType callbackType, string message)
        {
            switch (callbackType)
            {
                case ProviderCallbackType.ShowControl:
                    TopContent = _colorPickProvider.ShowControl;
                    break;
                case ProviderCallbackType.End:
                    TopContent = null;
                    if (_colorPickProvider.Result)
                    {
                        _color = _colorPickProvider.ResultColor;
                        UpdateUi();
                    }

                    _colorPickProvider = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }

        #region binding

        public ICommand CommandMouseDown { get; }

        [ValueBind]
        public Brush BrushColor
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Brush BrushGradientValue
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Brush BrushGradient
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Control TopContent
        {
            get => Get();
            set => Set(value);
        }

        #endregion
    }
}