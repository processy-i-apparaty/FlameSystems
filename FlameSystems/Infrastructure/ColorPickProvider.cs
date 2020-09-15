using System;
using System.Windows.Controls;
using System.Windows.Media;
using FlameSystems.Controls.Views;

namespace FlameSystems.Infrastructure
{
    internal class ColorPickProvider
    {
        private readonly Action<string, string, Control> _providerCallback;
        private const string ActionFireString = "COLOR_PICK_PROVIDER-COLOR_PICKER_CALLBACK";

        public bool Result { get; private set; }
        public Color ResultColor { get; private set; }

        public ColorPickProvider(Action<string, string, Control> providerCallback, Color initialColor)
        {
            _providerCallback = providerCallback;
            ActionFire.ActionFire.AddOrReplace(ActionFireString, new Action<bool, Color>(ColorPickerCallback),
                GetType());
            Exec(initialColor);
        }

        public void Exec(Color initialColor)
        {
            var picker = new ColorPickerView(initialColor, ActionFireString);
            _providerCallback.Invoke("picker-view", "", picker);
        }

        private void ColorPickerCallback(bool result, Color resultColor)
        {
            Result = result;
            ResultColor = resultColor;
            _providerCallback("end", "", null);
        }
    }
}