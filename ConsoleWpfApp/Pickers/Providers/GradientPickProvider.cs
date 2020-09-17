using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using ConsoleWpfApp.Pickers.Enums;
using ConsoleWpfApp.Pickers.Models;
using ConsoleWpfApp.Pickers.ViewModels;
using ConsoleWpfApp.Pickers.Views;

namespace ConsoleWpfApp.Pickers.Providers
{
    internal class GradientPickProvider
    {
        private GradientPickView _gradientPickView;
        private readonly Action<ProviderCallbackType, string> _providerCallback;
        public GradientMode GradientMode { get; }
        private GradientModel _gradientModel;
        private readonly double _gradientValue;
        private ColorPickProvider _colorPickProvider;
        private int _gradientId;

        public GradientPickProvider(Action<ProviderCallbackType, string> providerCallback, GradientModel gradientModel)
        {
            _providerCallback = providerCallback;
            _gradientModel = gradientModel.Copy();
            _gradientPickView =
                new GradientPickView(_gradientModel, GradientPickCallback);
            GradientMode = GradientMode.Edit;
        }


        public GradientPickProvider(Action<ProviderCallbackType, string> providerCallback, GradientModel gradientModel,
            double gradientValue)
        {
            _providerCallback = providerCallback;
            _gradientModel = gradientModel.Copy();
            _gradientValue = gradientValue;
            _gradientPickView = new GradientPickView(_gradientModel, _gradientValue, GradientPickCallback);
            GradientMode = GradientMode.Select;
        }

        private void GradientPickCallback(GradientCallbackType gradientCallbackType, object obj, double gradientValue)
        {
            switch (gradientCallbackType)
            {
                case GradientCallbackType.EndValueTrue:
                    ResultValue = gradientValue;
                    Result = true;
                    _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                    break;
                case GradientCallbackType.EndGradientTrue:
                    ResultGradientModel = (GradientModel) obj;
                    Result = true;
                    _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                    break;
                case GradientCallbackType.EndValueFalse:
                case GradientCallbackType.EndGradientFalse:
                    Result = false;
                    _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                    break;
                case GradientCallbackType.SelectColor:
                    _gradientId = (int) gradientValue;
                    _colorPickProvider = new ColorPickProvider(ColorPickProviderCallback, (Color) obj);
                    _colorPickProvider.Exec();
                    break;
                case GradientCallbackType.SelectValue:
                    ResultValue = gradientValue;
                    _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gradientCallbackType), gradientCallbackType, null);
            }
        }

        private void ColorPickProviderCallback(ProviderCallbackType providerCallbackType, string message)
        {
            switch (providerCallbackType)
            {
                case ProviderCallbackType.ShowControl:
                    ShowControl = _colorPickProvider.ShowControl;
                    _providerCallback.Invoke(ProviderCallbackType.ShowControl, string.Empty);
                    break;
                case ProviderCallbackType.End:
                    if (_colorPickProvider.Result)
                    {
                        var color = _colorPickProvider.ResultColor;
                        _gradientModel = GetGradientModel(_gradientPickView);
                        _gradientModel.ChangeColor(_gradientId, color);
                    }

                    _gradientPickView = new GradientPickView(_gradientModel, GradientPickCallback);
                    ShowControl = _gradientPickView;
                    _providerCallback.Invoke(ProviderCallbackType.ShowControl, string.Empty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(providerCallbackType), providerCallbackType, null);
            }
        }

        private GradientModel GetGradientModel(GradientPickView gradientPickView)
        {
            var dc = (GradientPickViewModel) _gradientPickView.DataContext;
            return dc.GradientModel;
        }

        public bool Result { get; private set; }
        public double ResultValue { get; private set; }
        public GradientModel ResultGradientModel { get; private set; }
        public Control ShowControl { get; private set; }

        public void Exec()
        {
            ShowControl = _gradientPickView;
            _providerCallback.Invoke(ProviderCallbackType.ShowControl, string.Empty);
        }
    }
}