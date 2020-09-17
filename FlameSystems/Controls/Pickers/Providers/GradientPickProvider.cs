using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FlameBase.Models;
using FlameSystems.Controls.Pickers.Enums;
using FlameSystems.Controls.Pickers.ViewModels;
using FlameSystems.Controls.Pickers.Views;

namespace FlameSystems.Controls.Pickers.Providers
{
    internal class GradientPickProvider
    {
        private readonly Action<ProviderCallbackType, string> _providerCallback;
        private ColorPickProvider _colorPickProvider;
        private int _gradientId;
        private GradientModel _gradientModel;
        private GradientPickView _gradientPickView;

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
            _gradientPickView = new GradientPickView(_gradientModel, gradientValue, GradientPickCallback);
            GradientMode = GradientMode.Select;
        }

        public GradientMode GradientMode { get; }

        public bool Result { get; private set; }
        public double ResultValue { get; private set; }
        public GradientModel ResultGradientModel { get; private set; }
        public Control ShowControl { get; private set; }

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

        private static GradientModel GetGradientModel(FrameworkElement gradientPickView)
        {
            var dc = (GradientPickViewModel) gradientPickView.DataContext;
            return dc.GradientModel;
        }

        public void Exec()
        {
            ShowControl = _gradientPickView;
            _providerCallback.Invoke(ProviderCallbackType.ShowControl, string.Empty);
        }
    }
}