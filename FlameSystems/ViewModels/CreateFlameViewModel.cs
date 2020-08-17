using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlameBase.Enums;
using FlameBase.FlameMath;
using FlameBase.Models;
using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.ViewModels
{
    internal class CreateFlameViewModel : Notifier
    {
        private readonly string[] _bindParameters1 =
            {"ShiftX", "ShiftY", "Zoom", "Rotation", "Symmetry", "ImageWidth", "ImageHeight"};

        private readonly string[] _directories =
        {
            $"{Environment.CurrentDirectory}\\flames",
            $"{Environment.CurrentDirectory}\\images",
            $"{Environment.CurrentDirectory}\\renders"
        };

        private readonly RenderActionsModel _renderActionsPack;
        private readonly RenderSettingsModel _renderSettings = new RenderSettingsModel();
        private ColorPickerView _colorPicker;
        private string _colorPikMode = "transformation";

        private FlameColorMode _flameColorMode;
        private string _flameName;
        private GradientModel _gradModel;
        private GradientPickerView _gradView;
        private int _transformationId;


        public CreateFlameViewModel()
        {
            InitBindings();
            InitActionFire();
        }

        #region methods

        private void SetColorModeForAllTransforms(FlameColorMode flameColorMode)
        {
            GradientModel gm = null;
            if (_gradModel == null) _gradModel = new GradientModel(Colors.Gray, Colors.Gray);
            if (flameColorMode == FlameColorMode.Gradient) gm = _gradModel;

            foreach (var transformView in Transforms)
            {
                var dc = (TransformViewModel) transformView.DataContext;
                dc.GradientModel = gm;
                dc.FlameColorMode = flameColorMode;
            }
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
            //TODO: set names for actionFire
            ActionFire.Add("UCreateRemoveTransformation", new Action<int>(ActRemoveTransformation), GetType());
            ActionFire.Add("UCreateCallRender", new Action<string>(ActCallRender), GetType());
            ActionFire.Add("UCreateSetTransformationColor",
                new Action<TransformViewModel>(ActSetTransformationColor), GetType());
            ActionFire.Add("UCreateSetTransformationColorGradient",
                new Action<TransformViewModel>(ActSetTransformationColorGradient), GetType());
            ActionFire.Add("UCreateColorGradientCallback", new Action<bool>(ActColorGradientCallback), GetType());


            ActionFire.Add("UCreateColorPickerCallback", new Action<bool, Color>(ActColorPickerCallback), GetType());
            ActionFire.Add("UCreatePikGradient", new Action<Color>(ActionCreatePikGradient), GetType());
        }

        #endregion

        #region command handlers

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

        #region Actions

        private void ActSetTransformationColor(TransformViewModel model)
        {
            _colorPikMode = "transformation";
            _colorPicker = new ColorPickerView(model.ColorBrush.Color);
            _transformationId = model.Id;
            TopContent = _colorPicker;
        }


        private void ActCallRender(string renderType)
        {
            if (Transforms.Count < 1) return;
            //            Debug.WriteLine($"[ActCallRender] {renderType}");
            RenderPackModel renderPack = null;
            GetDataForRender(out var transformations, out var variations, out var viewSettings);
            var render = true;
            var @continue = false;
            var draftMode = false;
            switch (renderType)
            {
                case "draft":
                    var renderSettings = new RenderSettingsModel(100, 10);
                    var aspect = viewSettings.ImageAspect;
                    viewSettings.ImageWidth = renderSettings.DraftImageSideWidth;
                    viewSettings.ImageHeight = (int) (viewSettings.ImageWidth * aspect);
                    renderPack = new RenderPackModel(transformations, variations, viewSettings, renderSettings,
                        _flameColorMode, _gradModel);
                    IsEnabledContinueRender = false;
                    draftMode = true;
                    // RenderMachine.DraftMode = true;
                    break;
                case "render":
                    renderPack = new RenderPackModel(transformations, variations, viewSettings, _renderSettings,
                        _flameColorMode, _gradModel);
                    // RenderMachine.DraftMode = false;
                    break;
                case "continue":
                    renderPack = new RenderPackModel(transformations, variations, viewSettings, _renderSettings,
                        _flameColorMode, _gradModel);
                    // RenderMachine.DraftMode = false;
                    @continue = true;
                    break;
                default:
                    render = false;
                    break;
            }

            //TODO: add RenderMachine
            // if (render) RenderMachine.Render(renderPack, _renderActionsPack, draftMode, @continue);
        }

        private void FreezeControls(bool state)
        {
            BindStorage.FreezeFor(state, _bindParameters1);
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var transformationView in Transforms)
                {
                    var vm = (TransformViewModel) transformationView.DataContext;
                    vm.Freeze(state);
                }
            });
        }


        private void GetDataForRender(out TransformModel[] transforms, out VariationModel[] variations,
            out ViewSettingsModel viewSettings)
        {
            viewSettings =
                new ViewSettingsModel(ImageWidth, ImageHeight, ShiftX, ShiftY, Zoom, RotationRadians, Symmetry);


            variations = new VariationModel[Transforms.Count];
            transforms = new TransformModel[Transforms.Count];
            for (var i = 0; i < Transforms.Count; i++)
            {
                var dataContext = (TransformViewModel) Transforms[i].DataContext;
                variations[i] = dataContext.GetVariation;
                transforms[i] = dataContext.GetTransform;
            }
        }

        private void Act(string name, object obj)
        {
            //TODO: add RenderMachine
            // ActRenderSetState(RenderMachine.StateRenderEnded);

            ActCallRender("draft");
        }

        private void ActRemoveTransformation(int id)
        {
            var transformation =
                Transforms.FirstOrDefault(x => ((TransformViewModel) x.DataContext).Id == id);
            if (transformation == null) return;
            Transforms.Remove(transformation);

            //TODO: add RenderMachine
            // ActRenderSetState(RenderMachine.StateRenderEnded);

            if (Transforms.Count > 0)
            {
                ActCallRender("draft");
            }
        }

        private void ActRenderSetMessage(string message)
        {
            ActionFire.Invoke("WMainSetTextBottom", message);
        }

        private void ActRenderSetImage(BitmapSource img)
        {
            ImageSource = img;
        }

        #region grad-view

        private void EditGradientHandler(object obj)
        {
            _gradView = new GradientPickerView(_gradModel);
            TopContent = _gradView;
        }

        private void ActSetTransformationColorGradient(TransformViewModel model)
        {
            _transformationId = model.Id;
            _gradView = new GradientPickerView(_gradModel, model.ColorPosition);
            TopContent = _gradView;
        }

        private void ActColorGradientCallback(bool isOk)
        {
            TopContent = null;
            if (!isOk) return;
            var dc = (GradientPickerViewModel) _gradView.DataContext;
            switch (dc.GradientMode)
            {
                case GradientMode.Edit:
                    _gradModel = dc.GradientModel;
                    //amhere
                    SetTransformationColorMode(_flameColorMode);
                    break;
                case GradientMode.Select:
                    var t = (TransformViewModel) Transforms.FirstOrDefault(x =>
                            ((TransformViewModel) x.DataContext).Id == _transformationId)
                        ?.DataContext;
                    if (t != null) t.ColorPosition = dc.GetColorPosition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ActColorPickerCallback(bool result, Color color)
        {
            _colorPicker.DataContext = null;
            _colorPicker = null;
            TopContent = null;

            switch (_colorPikMode)
            {
                case "transformation":
                    if (!result) return;
                    var t = (TransformViewModel) Transforms
                        .FirstOrDefault(x => ((TransformViewModel) x.DataContext).Id == _transformationId)
                        ?.DataContext;
                    if (t == null) return;
                    t.FColor = color;
                    break;
                case "gradient":
                    if (result)
                    {
                        TopContent = _gradView;

                        //TODO: set name for action
                        ActionFire.Invoke("UColorGradPickerCallback", color);
                    }
                    else
                    {
                        TopContent = _gradView;
                    }

                    break;
            }
        }

        #endregion

        #endregion

        #region methods

        private void SetTransformationColorMode(FlameColorMode flameColorMode)
        {
            GradientModel gm = null;
            if (_gradModel == null) _gradModel = new GradientModel(Colors.Gray, Colors.Gray);
            if (flameColorMode == FlameColorMode.Gradient) gm = _gradModel;

            foreach (var uTransformationView in Transforms)
            {
                var dc = (TransformViewModel) uTransformationView.DataContext;
                dc.GradientModel = gm;
                dc.FlameColorMode = flameColorMode;
            }
        }

        private void ActionCreatePikGradient(Color color)
        {
            if (TopContent.GetType() != typeof(GradientPickerView)) return;
            _colorPikMode = "gradient";
            _colorPicker = new ColorPickerView(color);
            TopContent = _colorPicker;
        }

        private void Dirs()
        {
            foreach (var directory in _directories)
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
        }

        private void ActRenderSetState(string obj)
        {
            switch (obj)
            {
                //TODO: RenderMachine
                // case RenderMachine.StateRenderStarted:
                //     IsEnabledNew = false;
                //     IsEnabledLoad = false;
                //     IsEnabledSave = false;
                //     IsEnabledLoadRender = false;
                //     IsEnabledSaveRender = false;
                //     IsEnabledContinueRender = false;
                //     IsEnabledStartRender = false;
                //     IsEnabledRenderSettings = false;
                //     IsEnabledStopRender = true;
                //     IsButtonAddTransformationEnabled = false;
                //     FreezeControls(true);
                //     break;
                // case RenderMachine.StateRenderEnded:
                //     IsEnabledNew = true;
                //     IsEnabledLoad = true;
                //     IsEnabledSave = Transforms.Count > 0;
                //     IsEnabledLoadRender = true;
                //     IsEnabledSaveRender = RenderMachine.HasRender;
                //     IsEnabledContinueRender = RenderMachine.HasRender;
                //     IsEnabledStartRender = Transforms.Count > 0;
                //     IsEnabledRenderSettings = true;
                //     IsEnabledStopRender = false;
                //     FreezeControls(false);
                //     IsButtonAddTransformationEnabled = true;
                //     if (RenderMachine.HasRender)
                //         RenderMachine.SaveImage($"{Environment.CurrentDirectory}\\images", "img", _flameColorMode);
                //     break;
            }
        }

        private void Init()
        {
            BindStorage.SetActionFor(Act, _bindParameters1);

            //TODO: set names for actions
            ActionFire.Invoke("WMainSetVersion", 1, 1, 1);
            ActionFire.Invoke("WMainSetTextBottom", "app started...");
        }

        #endregion
    }
}