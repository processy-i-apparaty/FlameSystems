using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlameBase.Enums;
using FlameBase.FlameMath;
using FlameBase.Models;
using FlameBase.RenderMachine;
using FlameBase.RenderMachine.Models;
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
            {"ShiftX", "ShiftY", "Zoom", "Rotation", "Symmetry", "ImageWidth", "ImageHeight", "BackColor"};

        private readonly RenderSettingsModel _renderSettings = new RenderSettingsModel();
        private ColorPickerView _colorPicker;

        private string _colorPikMode = "transform";

        private FlameColorMode _flameColorMode;
        private string _flameName;
        private GradientModel _gradientModel;
        private GradientPickerView _gradView;


        private RenderActionsModel _renderActionsPack;
        private int _transformId;


        public CreateFlameViewModel()
        {
            InitBindings();
            InitActions();
            InitDirectories();
            InitStrings();

            ActRenderSetState(RenderMachine.StateRenderEnded);
        }

        #region methods

        private void SetColorModeForAllTransforms(FlameColorMode flameColorMode)
        {
            GradientModel gm = null;
            if (_gradientModel == null) _gradientModel = new GradientModel(Colors.Gray, Colors.Gray);
            if (flameColorMode == FlameColorMode.Gradient) gm = _gradientModel;

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
            BackColor = Brushes.Black;
        }

        private void InitActions()
        {
            var thisType = GetType();
            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-TRANSFORM_REMOVE", new Action<int>(ActionTransformRemove),
                thisType);
            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-CALL_RENDER", new Action<string>(ActionCallRender),
                thisType);

            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_COLOR",
                new Action<TransformViewModel>(ActionTransformPickColor), thisType);
            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_COLOR_CALLBACK",
                new Action<bool, Color>(ActionTransformPickColorCallback), thisType);

            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_GRADIENT_COLOR",
                new Action<TransformViewModel>(ActionTransformPickGradientColor), thisType);
            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_GRADIENT_CALLBACK",
                new Action<bool>(ActionTransformPickGradientCallback), thisType);

            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-PICK_GRADIENT", new Action<Color>(ActionPickGradient),
                thisType);

            _renderActionsPack = new RenderActionsModel(ActRenderSetImage, ActRenderSetMessage, ActRenderSetState);
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
            switch ((string) obj)
            {
                case "new":
                    New();
                    break;
                case "load":
                    LoadFlame();
                    break;
                case "save":
                    SaveFlame();
                    break;
                case "loadRender":
                    LoadRender();
                    break;
                case "saveRender":
                    SaveRender();
                    break;
                case "startRender":
                    ActionCallRender("render");
                    break;
                case "stopRender":
                    RenderMachine.RenderStop();
                    break;
                case "continueRender":
                    ActionCallRender("continue");
                    break;
                case "renderSettings":
                    RenderSettings();
                    break;
                case "addTransform":
                    AddTransform();
                    break;
                case "back color":
                    SelectBackColor();
                    break;
                case "toPostPro":
                    ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_WINDOW_CONTENT_BY_PARAMS", "PostFlame", null);
                    break;
            }
        }

        private void SelectBackColor()
        {
            _colorPikMode = "back color";
            _colorPicker = new ColorPickerView(BackColor.Color);
            TopContent = _colorPicker;
        }


        private void CreateFileDialog(FileViewType fileViewType)
        {
            var dialog = new FileView();
            var vm = (FileViewModel) dialog.DataContext;
            vm.Set(fileViewType, ActDialogResult);
            TopContent = dialog;
        }

        private void SaveFlame()
        {
            CreateFileDialog(FileViewType.SaveFlame);
        }

        private void LoadFlame()
        {
            CreateFileDialog(FileViewType.LoadFlame);
        }

        private void SaveRender()
        {
            CreateFileDialog(FileViewType.SaveRender);
        }

        private void LoadRender()
        {
            CreateFileDialog(FileViewType.LoadRender);
        }


        private async void New()
        {
            RenderMachine.RenderStop();
            await Task.Run(() =>
            {
                while (RenderMachine.IsRendering) Thread.Sleep(10);
            });
            Transforms.Clear();
            RenderMachine.DestroyDisplay();
            ImageSource = null;
            ActRenderSetState(RenderMachine.StateRenderEnded);
            ActRenderSetMessage("New...");
        }


        private void RenderSettings()
        {
            var uRenderSettingsView = new RenderSettingsView();
            var vm = (RenderSettingsViewModel) uRenderSettingsView.DataContext;
            vm.Set(_renderSettings, ActionRenderSettingsCallback);
            TopContent = uRenderSettingsView;
        }

        private int AddTransform(bool act = true)
        {
            var utv = new TransformView();
            var dc = (TransformViewModel) utv.DataContext;
            var id = dc.Id = GiveIdModel.Get;

            switch (_flameColorMode)
            {
                case FlameColorMode.Color:
                    dc.GradientModel = null;
                    break;
                case FlameColorMode.Gradient:
                    dc.GradientModel = _gradientModel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            dc.FlameColorMode = _flameColorMode;
            Transforms.Add(utv);
            //TODO: name of act
            if (act) Act("AddTransformation", id);
            return id;
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

        [ValueBind]
        public SolidColorBrush BackColor
        {
            get => Get();
            set => Set(value);
        }

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

        [ValueBind(true)]
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

        private void ActionRenderSettingsCallback()
        {
            TopContent = null;
        }

        private void ActionTransformPickColor(TransformViewModel model)
        {
            _colorPikMode = "transform";
            _colorPicker = new ColorPickerView(model.ColorBrush.Color);
            _transformId = model.Id;
            TopContent = _colorPicker;
        }


        private void ActionCallRender(string renderType)
        {
            if (Transforms.Count < 1) return;
            //            Debug.WriteLine($"[ActCallRender] {renderType}");
            RenderPackModel renderPack = null;
            GetDataForRender(out var transformations, out var variations, out var viewSettings, out var gradientModel);
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
                        _flameColorMode, gradientModel);
                    IsEnabledContinueRender = false;
                    draftMode = true;
                    break;
                case "render":
                    renderPack = new RenderPackModel(transformations, variations, viewSettings, _renderSettings,
                        _flameColorMode, gradientModel);
                    break;
                case "continue":
                    renderPack = new RenderPackModel(transformations, variations, viewSettings, _renderSettings,
                        _flameColorMode, gradientModel);
                    @continue = true;
                    break;
                default:
                    render = false;
                    break;
            }

            if (render) RenderMachine.Render(renderPack, _renderActionsPack, draftMode, @continue);
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
            out ViewSettingsModel viewSettings, out GradientModel gradientModel)
        {
            viewSettings =
                new ViewSettingsModel(ImageWidth, ImageHeight, ShiftX, ShiftY, Zoom, RotationRadians, Symmetry,
                    BackColor.Color);


            variations = new VariationModel[Transforms.Count];
            transforms = new TransformModel[Transforms.Count];
            for (var i = 0; i < Transforms.Count; i++)
            {
                var dataContext = (TransformViewModel) Transforms[i].DataContext;
                variations[i] = dataContext.GetVariation;
                transforms[i] = dataContext.GetTransform;
            }

            gradientModel = null;
            if (_flameColorMode == FlameColorMode.Gradient) gradientModel = _gradientModel;
        }

        private void Act(string name, object obj)
        {
            ActRenderSetState(RenderMachine.StateRenderEnded);
            ActionCallRender("draft");
        }

        private void ActionTransformRemove(int id)
        {
            var transformation =
                Transforms.FirstOrDefault(x => ((TransformViewModel) x.DataContext).Id == id);
            if (transformation == null) return;
            Transforms.Remove(transformation);

            ActRenderSetState(RenderMachine.StateRenderEnded);

            if (Transforms.Count > 0) ActionCallRender("draft");
        }

        private void ActRenderSetMessage(string message)
        {
            ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_BOTTOM_STRING", message);
        }

        private void ActRenderSetImage(BitmapSource img)
        {
            ImageSource = img;
        }

        private void ActDialogResult(bool result, FileViewType fileDialogType, string path)
        {
            TopContent = null;
            if (!result) return;

            string flameModelJson;
            string tempDir;

            switch (fileDialogType)
            {
                case FileViewType.LoadFlame:
                    if (!File.Exists(path)) return;
                    flameModelJson = File.ReadAllText(path);
                    var model = JsonFlamesModel.GetFlameModel(flameModelJson);
                    if (model == null) return;
                    SetEnvironmentFromFlameModel(model);
                    _flameName = Path.GetFileNameWithoutExtension(path);
                    RenderMachine.RenderStop();
                    ActRenderSetState(RenderMachine.StateRenderEnded);
                    ActionCallRender("draft");
                    break;
                case FileViewType.SaveFlame:
                    RenderMachine.RenderStop();

                    flameModelJson = GetFlameModelJson();
                    File.WriteAllText(path, flameModelJson);
                    _flameName = Path.GetFileNameWithoutExtension(path);
                    ActRenderSetState(RenderMachine.StateRenderEnded);
                    ActRenderSetMessage($"{_flameName} saved");
                    break;
                case FileViewType.SaveRender:
                    var renderName = Path.GetFileNameWithoutExtension(path);
                    tempDir = CreateTempDir(renderName);

                    flameModelJson = GetFlameModelJson();
                    BinaryFlamesModel.SaveObject(RenderMachine.Display.GetArrayCopy(), $"{tempDir}\\logDisplay.bin");
                    File.WriteAllText($"{tempDir}\\flame.txt", flameModelJson);
                    ZipFlamesModel.CompressDirectory(tempDir, path);
                    RemoveTempDir();
                    break;
                case FileViewType.LoadRender:
                    if (!File.Exists(path)) return;
                    tempDir = CreateTempDir(_flameName);
                    ZipFlamesModel.DecompressToDirectory(path, tempDir);
                    var display = (uint[,,]) BinaryFlamesModel.LoadObject($"{tempDir}\\logDisplay.bin");
                    var json = File.ReadAllText($"{tempDir}\\flame.txt");
                    var flameModel = JsonFlamesModel.GetFlameModel(json);
                    SetEnvironmentFromFlameModel(flameModel);
                    _flameName = Path.GetFileNameWithoutExtension(path);
                    RenderMachine.RenderStop();
                    ActRenderSetState(RenderMachine.StateRenderEnded);

                    RenderMachine.LoadDisplay(display, flameModel.BackColor);

                    GradientModel gradientModel = null;
                    double[] gradientValues = null;
                    if (flameModel.GradientPack != null)
                    {
                        var gm = new GradientModel(flameModel.GradientPack);
                        gradientModel = gm.Copy();
                        gradientValues = flameModel.FunctionColorPositions.ToArray();
                    }

                    ImageSource = RenderMachine.GetImage(flameModel.FunctionColors.ToArray(), gradientValues,
                        gradientModel);
                    ActRenderSetState(RenderMachine.StateRenderEnded, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileDialogType), fileDialogType, null);
            }
        }

        private void SetEnvironmentFromFlameModel(FlameModel flameModel)
        {
            if (flameModel == null) return;
            Transforms.Clear();
            RadioColor = true;
            InitFromModel(flameModel);
        }

        private static string CreateTempDir(string flameName)
        {
            var tempPath = $"{Path.GetTempPath()}flame-systems-temporary\\{flameName}";
            if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

        private static void RemoveTempDir()
        {
            var tempPath = $"{Path.GetTempPath()}flame-systems-temporary";
            try
            {
                if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private string GetFlameModelJson()
        {
            GetDataForRender(out var transforms, out var variations, out var viewSettings, out var gradientModel);
            return JsonFlamesModel.GetFlameModelJson(transforms, variations, viewSettings, gradientModel);
        }


        #region grad-view

        private void EditGradientHandler(object obj)
        {
            _gradView = new GradientPickerView(_gradientModel);
            TopContent = _gradView;
        }

        private void ActionTransformPickGradientColor(TransformViewModel model)
        {
            _transformId = model.Id;
            _gradView = new GradientPickerView(_gradientModel, model.ColorPosition);
            TopContent = _gradView;
        }

        private void ActionTransformPickGradientCallback(bool isOk)
        {
            TopContent = null;
            if (!isOk) return;
            var dc = (GradientPickerViewModel) _gradView.DataContext;
            switch (dc.GradientMode)
            {
                case GradientMode.Edit:
                    _gradientModel = dc.GradientModel;
                    //amhere
                    SetTransformationColorMode(_flameColorMode);
                    break;
                case GradientMode.Select:
                    var t = (TransformViewModel) Transforms.FirstOrDefault(x =>
                            ((TransformViewModel) x.DataContext).Id == _transformId)
                        ?.DataContext;
                    if (t != null) t.ColorPosition = dc.GetColorPosition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ActionTransformPickColorCallback(bool result, Color color)
        {
            _colorPicker.DataContext = null;
            _colorPicker = null;
            TopContent = null;

            switch (_colorPikMode)
            {
                case "transform":
                    if (!result) return;
                    var t = (TransformViewModel) Transforms
                        .FirstOrDefault(x => ((TransformViewModel) x.DataContext).Id == _transformId)
                        ?.DataContext;
                    if (t == null) return;
                    t.FColor = color;
                    break;
                case "gradient":
                    if (result)
                    {
                        TopContent = _gradView;
                        ActionFire.Invoke("GRADIENT_PICKER_VIEWMODEL-CALLBACK", color);
                    }
                    else
                    {
                        TopContent = _gradView;
                    }

                    break;
                case "back color":
                    if (!result) return;
                    BackColor = new SolidColorBrush(color);
                    break;
            }
        }

        #endregion

        #endregion

        #region methods

        private void SetTransformationColorMode(FlameColorMode flameColorMode)
        {
            GradientModel gm = null;
            if (_gradientModel == null) _gradientModel = new GradientModel(Colors.Gray, Colors.Gray);
            if (flameColorMode == FlameColorMode.Gradient) gm = _gradientModel;

            foreach (var uTransformationView in Transforms)
            {
                var dc = (TransformViewModel) uTransformationView.DataContext;
                dc.GradientModel = gm;
                dc.FlameColorMode = flameColorMode;
            }
        }

        private void ActionPickGradient(Color color)
        {
            if (TopContent.GetType() != typeof(GradientPickerView)) return;
            _colorPikMode = "gradient";
            _colorPicker = new ColorPickerView(color);
            TopContent = _colorPicker;
        }

        private static void InitDirectories()
        {
            var directories = Directories.GetAll;
            foreach (var directory in directories)
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
        }


        private void ActRenderSetState(string obj, bool saveImage = true)
        {
            switch (obj)
            {
                case RenderMachine.StateRenderStarted:
                    IsEnabledNew = false;
                    IsEnabledLoad = false;
                    IsEnabledSave = false;
                    IsEnabledLoadRender = false;
                    IsEnabledSaveRender = false;
                    IsEnabledContinueRender = false;
                    IsEnabledStartRender = false;
                    IsEnabledRenderSettings = false;
                    IsEnabledStopRender = true;
                    IsEnabledAddTransform = false;
                    FreezeControls(true);
                    break;
                case RenderMachine.StateRenderEnded:
                    IsEnabledNew = true;
                    IsEnabledLoad = true;
                    IsEnabledSave = Transforms.Count > 0;
                    IsEnabledLoadRender = true;
                    IsEnabledSaveRender = RenderMachine.HasRender;
                    IsEnabledContinueRender = RenderMachine.HasRender;
                    IsEnabledStartRender = Transforms.Count > 0;
                    IsEnabledRenderSettings = true;
                    IsEnabledStopRender = false;
                    FreezeControls(false);
                    IsEnabledAddTransform = true;

                    if (RenderMachine.HasRender && saveImage)
                    {
                        Debug.WriteLine("Saving Rendered Image");
                        RenderMachine.SaveImage(Directories.Images, "img", _flameColorMode);
                        Debug.WriteLine("Saving Rendered Image end.");
                    }

                    break;
            }
        }

        private void InitStrings()
        {
            BindStorage.SetActionFor(Act, _bindParameters1);

            //TODO: set names for actions
            ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_VERSION", 1, 1, 1);
            ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_BOTTOM_STRING", "app started...");
        }


        private void LoadCoefficients(double[] coefficients, double probability, Color color, double colorPosition,
            int variationId,
            double[] parameters, double weight = 1.0)
        {
            var id = AddTransform(false);
            var model = new TransformModel();
            model.SetFromCoefficients(coefficients, probability, color, colorPosition);
            var utv = (TransformViewModel) Transforms
                .FirstOrDefault(x => ((TransformViewModel) x.DataContext).Id == id)
                ?.DataContext;
            if (utv == null) return;
            utv.GradientModel = _gradientModel;
            utv.SetTransformation(model);
            utv.SetVariation(variationId, parameters, weight);
        }

        private void InitFromModel(FlameModel model)
        {
            var hasParameters = model.Parameters != null;
            var hasWeights = model.Weights != null;
            var hasGradient = model.GradientPack != null;

            _gradientModel = hasGradient ? new GradientModel(model.GradientPack) : null;

            for (var i = 0; i < model.Coefficients.Count; i++)
            {
                var c = new double[6];
                var probability = model.Coefficients[i][6];
                Array.Copy(model.Coefficients[i], c, 6);
                var color = model.FunctionColors[i];
                var variationId = model.VariationIds[i];
                double[] parameters = null;
                var colorPosition = .5;
                var weight = 1.0;
                if (hasParameters)
                    parameters = model.Parameters[i];
                if (hasWeights)
                    weight = model.Weights[i];
                if (hasGradient)
                    colorPosition = model.FunctionColorPositions[i];

                LoadCoefficients(c, probability, color, colorPosition, variationId, parameters, weight);
            }

            LoadViewSettings(model);
            if (hasGradient) RadioGradient = true;
        }

        private void LoadViewSettings(FlameModel model)
        {
            BindStorage.TurnActionFor(false, _bindParameters1);
            ShiftX = model.ViewShiftX;
            ShiftY = model.ViewShiftY;
            RotationRadians = model.Rotation;
            Zoom = model.ViewZoom;
            Symmetry = model.Symmetry;
            ImageWidth = model.ImageWidth;
            ImageHeight = model.ImageHeight;
            BackColor = new SolidColorBrush(model.BackColor);
            BindStorage.TurnActionFor(true, _bindParameters1);
        }

        #endregion
    }
}