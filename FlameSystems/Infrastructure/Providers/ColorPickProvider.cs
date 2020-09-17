using System;
using System.Windows.Controls;
using System.Windows.Media;
using FlameSystems.Controls.Views;

namespace FlameSystems.Infrastructure.Providers
{
    internal class ColorPickProvider
    {
        private const string ActionFireString = "COLOR_PICK_PROVIDER-COLOR_PICKER_CALLBACK";
        private readonly Color _initialColor;
        private readonly Action<ProviderEnums.CallbackType, string> _providerCallback;
        private ColorPickerView _colorPickView;

        public ColorPickProvider(Action<ProviderEnums.CallbackType, string> providerCallback, Color initialColor)
        {
            _providerCallback = providerCallback;
            ActionFire.ActionFire.AddOrReplace(ActionFireString, new Action<bool, Color>(ColorPickerCallback),
                GetType());
            _initialColor = initialColor;
        }

        public bool Result { get; private set; }
        public Color ResultColor { get; private set; }
        public Control ShowControl => _colorPickView;

        public void Exec()
        {
            _colorPickView = new ColorPickerView(_initialColor, ActionFireString);
            _providerCallback.Invoke(ProviderEnums.CallbackType.ShowControl, string.Empty);
        }

        private void ColorPickerCallback(bool result, Color resultColor)
        {
            Result = result;
            ResultColor = resultColor;
            _colorPickView = null;
            _providerCallback(ProviderEnums.CallbackType.End, string.Empty);
        }
    }
}