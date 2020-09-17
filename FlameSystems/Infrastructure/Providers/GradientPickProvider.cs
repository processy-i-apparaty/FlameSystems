using System;
using System.Windows.Controls;
using System.Windows.Media;
using FlameBase.Enums;
using FlameBase.Models;
using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views;

namespace FlameSystems.Infrastructure.Providers
{
    internal class GradientPickProvider
    {
        private const string ActionFireString1 = "GRADIENT_PICK_PROVIDER-GRADIENT_PICKER_CALLBACK";
        private const string ActionFireString2 = "GRADIENT_PICK_PROVIDER-GRADIENT_PICKER_COLOR";
        public GradientPickerType PickerType { get; private set; }
        private GradientPickerView _gradientPickerView;
        private double _gradientValue;

        private Action<ProviderEnums.CallbackType, string> _providerCallback;

        public GradientPickProvider(Action<ProviderEnums.CallbackType, string> providerCallback,
            GradientModel gradientModel)
        {
            Init(providerCallback, gradientModel);
        }

        public GradientPickProvider(Action<ProviderEnums.CallbackType, string> providerCallback,
            GradientModel gradientModel, double gradientValue)
        {
            Init(providerCallback, gradientModel, gradientValue);
        }

        public bool Result { get; private set; }
        public double ResultGradientPosition { get; private set; }
        public GradientModel ResultGradientModel { get; private set; }
        public Control ShowControl => _gradientPickerView;

        private void Init(Action<ProviderEnums.CallbackType, string> providerCallback,
            GradientModel gradientModel, double gradientValue = -1.0)
        {
            _providerCallback = providerCallback;
            _gradientValue = gradientValue;
            ResultGradientModel = gradientModel;
            ActionFire.ActionFire.AddOrReplace(ActionFireString1, new Action<bool>(GradientPickCallback), GetType());
            ActionFire.ActionFire.AddOrReplace(ActionFireString2, new Action<Color>(GradientPickColorCallback),
                GetType());
            PickerType = gradientValue < 0.0 ? GradientPickerType.Gradient : GradientPickerType.Position;
        }

        private void GradientPickColorCallback(Color obj)
        {
            _providerCallback.Invoke(ProviderEnums.CallbackType.ShowControl, obj.ToString());
        }

        private void GradientPickCallback(bool result)
        {
            Result = result;
            if (!result)
            {
                _gradientPickerView = null;
                _providerCallback.Invoke(ProviderEnums.CallbackType.End, string.Empty);
                return;
            }

            var dc = (GradientPickerViewModel) _gradientPickerView.DataContext;
            switch (dc.GradientMode)
            {
                case GradientMode.Edit:
                    ResultGradientModel = dc.GradientModel;
                    break;
                case GradientMode.Select:
                    ResultGradientPosition = dc.GetColorPosition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _gradientPickerView = null;
            _providerCallback.Invoke(ProviderEnums.CallbackType.End, string.Empty);
        }


        public void Exec()
        {
            switch (PickerType)
            {
                case GradientPickerType.Gradient:
                    _gradientPickerView =
                        new GradientPickerView(ResultGradientModel, ActionFireString1, ActionFireString2);
                    _providerCallback.Invoke(ProviderEnums.CallbackType.ShowControl, string.Empty);
                    break;
                case GradientPickerType.Position:
                    _gradientPickerView =
                        new GradientPickerView(ResultGradientModel, _gradientValue, ActionFireString1,
                            ActionFireString2);
                    _providerCallback.Invoke(ProviderEnums.CallbackType.ShowControl, string.Empty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum GradientPickerType
        {
            Gradient,
            Position
        }
    }
}