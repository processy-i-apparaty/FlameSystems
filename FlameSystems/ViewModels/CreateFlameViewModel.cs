using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FlameBase.Enums;
using FlameBase.FlameMath;
using FlameBase.Models;
using FlameSystems.Controls.Views;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.ViewModels
{
    internal class CreateFlameViewModel : Notifier
    {
        private readonly string[] _bindParameters1 =
            {"ShiftX", "ShiftY", "Zoom", "Rotation", "Symmetry", "ImageWidth", "ImageHeight"};

        private FlameColorMode _flameColorMode;


        public CreateFlameViewModel()
        {
            InitBindings();
            InitActionFire();
        }

        #region methods

        private void SetColorModeForAllTransforms(FlameColorMode flameColorMode)
        {
            // GradModel gm = null;
            // if (_gradModel == null) _gradModel = new GradModel(Colors.Gray, Colors.Gray);
            // if (flameColorMode == FlameColorMode.Gradient) gm = _gradModel;
            //
            // foreach (var uTransformationView in Transforms)
            // {
            //     var dc = (UTransformationViewModel)uTransformationView.DataContext;
            //     dc.GradModel = gm;
            //     dc.FlameColorMode = flameColorMode;
            // }
        }

        #endregion
        
        #region init methods

        private void InitBindings()
        {
            MultiCommand = new RelayCommand(MultiCommandHandler);
            CommandColorRadioChecked = new RelayCommand(ColorRadioCheckedHandler);
            CommandEditGradient = new RelayCommand(EditGradientHandler);
            Transforms = new ObservableCollection<TransformView>();
            RadioColor = true;
        }

        private void InitActionFire()
        {
        }

        #endregion

        #region command handlers

        private void EditGradientHandler(object obj)
        {
        }

        private void ColorRadioCheckedHandler(object obj)
        {
            var radio = (RadioButton) obj;
            if (radio.IsChecked != true) return;
            var name = radio.Name;
            switch (name)
            {
                case "RadioColor":
                    _flameColorMode = FlameColorMode.Color;
                    break;
                case "RadioGradient":
                    _flameColorMode = FlameColorMode.Gradient;
                    break;
            }

            IsEnabledGradientMode = name == "RadioGradient";
            SetColorModeForAllTransforms(_flameColorMode);
        }

        private void MultiCommandHandler(object obj)
        {
        }

        #endregion

        #region bindings commands

        public ICommand CommandColorRadioChecked { get; set; }
        public ICommand CommandEditGradient { get; set; }
        public ICommand MultiCommand { get; set; }

        #endregion

        #region bindings other

        public ObservableCollection<TransformView> Transforms { get; set; }

        [ValueBind]
        public BitmapSource ImageSource
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

        #endregion

        #region bindings flame parameters

        [ValueBind(0.0, -10.0, 10.0)]
        public double ShiftX
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(0.0, -10.0, 10.0)]
        public double ShiftY
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1.0, 0.001, 10.0)]
        public double Zoom
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(0.0, 0.0, 360.0, true)]
        public double Rotation
        {
            get => Get();
            set => Set(value);
        }

        public double RotationRadians
        {
            get => Rotation * Trigonometry.ToRadians;
            set => Rotation = value * Trigonometry.ToDegrees;
        }

        [ValueBind(1, 1, 16)]
        public int Symmetry
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1920, 16, 15000)]
        public int ImageWidth
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1080, 16, 15000)]
        public int ImageHeight
        {
            get => Get();
            set => Set(value);
        }

        #endregion

        #region bindings buttons isEnabled

        [ValueBind]
        public bool IsEnabledNew
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledLoad
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledSave
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledStartRender
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledStopRender
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledContinueRender
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledLoadRender
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledSaveRender
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledRenderSettings
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledAddTransform
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool IsEnabledGradientMode
        {
            get => Get();
            set => Set(value);
        }

        #endregion
    }
}