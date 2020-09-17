using System;
using System.Windows.Controls;
using System.Windows.Media;
using FlameSystems.Controls.Pickers.Enums;
using FlameSystems.Controls.Pickers.Views;

namespace FlameSystems.Controls.Pickers.Providers
{
    internal class ColorPickProvider
    {
        private readonly ColorPickView _colorPickView;
        private readonly Action<ProviderCallbackType, string> _providerCallback;

        public ColorPickProvider(Action<ProviderCallbackType, string> providerCallback, Color initialColor)
        {
            _providerCallback = providerCallback;
            _colorPickView = new ColorPickView(initialColor, ColorPickCallback);
        }

        public bool Result { get; private set; }
        public Color ResultColor { get; private set; }
        public Control ShowControl => _colorPickView;

        public void Exec()
        {
            _providerCallback.Invoke(ProviderCallbackType.ShowControl, string.Empty);
        }

        private void ColorPickCallback(bool result, Color resultColor)
        {
            Result = result;
            ResultColor = resultColor;
            _providerCallback(ProviderCallbackType.End, string.Empty);
        }
    }
}