using System;
using System.Collections.Generic;
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
using FlameSystems.Controls.Pickers.Enums;
using FlameSystems.Controls.Pickers.Providers;
using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views;
using FlameSystems.Controls.Views.Trans;
using FlameSystems.Enums;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.ViewModels
{
    internal class CreateFlameViewModel : Notifier
    {
        //todo: refactor Create Flame ViewModel
        private readonly string[] _bindParameters1 =
            {"ShiftX", "ShiftY", "Zoom", "Rotation", "Symmetry", "ImageWidth", "ImageHeight", "BackColor"};

        private readonly RenderSettingsModel _renderSettings = new RenderSettingsModel();

        private FlameColorMode _flameColorMode;
        private string _flameName;
        private GradientModel _gradientModel;
        private RenderActionsModel _renderActionsPack;
        private int _transformId;

        public CreateFlameViewModel()
        {
            InitBindings();
            InitCommands();
            InitActions();
            InitDirectories();
            InitStrings();

            ActRenderSetState(RenderMachine.StateRenderEnded);
        }

        #region init methods

        private void InitBindings()
        {
            //Transforms = new ObservableCollection<TransformView>();
            Transforms = new ObservableCollection<IView>();
            RadioColor = true;
            BackColor = Brushes.Black;
        }

        private void InitCommands()
        {
            MultiCommand = new RelayCommand(HandlerMain);
            CommandColorRadioChecked = new RelayCommand(ColorRadioCheckedHandler);
            CommandEditGradient = new RelayCommand(EditGradientHandler);
            CommandPanelIsEnabledChanged = new RelayCommand(HandlerPanelIsEnabledChanged);
        }


        private void InitActions()
        {
            var thisType = GetType();
            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-TRANSFORM_REMOVE", new Action<int>(ActionTransformRemove),
                thisType);
            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-CALL_RENDER", new Action<string>(MainActionCallRender),
                thisType);

            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_COLOR",
                new Action<int, Color>(ActionTransformPickColor), thisType);

            ActionFire.AddOrReplace("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_GRADIENT_COLOR",
                new Action<int, double>(ActionTransformPickGradientColor), thisType);

            _renderActionsPack = new RenderActionsModel(ActRenderSetImage, ActRenderSetMessage, ActRenderSetState);
        }

        #endregion

        #region command handlers

        private void HandlerPanelIsEnabledChanged(object obj)
        {
            //todo: handler Panel Is Enabled Changed
            switch (((Panel) obj).Name)
            {
                case "Panel0":
                    break;
                case "Panel1":
                    break;
                case "Panel2":
                    break;
                case "Panel3":
                    break;
                case "Panel4":
                    break;
            }
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

        #endregion

        #region main

        private void HandlerMain(object obj)
        {
            switch ((string) obj)
            {
                case "new":
                    MainNew();
                    break;
                case "load":
                    MainLoadFlame();
                    break;
                case "save":
                    MainSaveFlame();
                    break;
                case "loadRender":
                    MainLoadRender();
                    break;
                case "saveRender":
                    MainSaveRender();
                    break;
                case "startRender":
                    MainActionCallRender("render");
                    break;
                case "stopRender":
                    RenderMachine.MainRenderStop();
                    break;
                case "continueRender":
                    MainActionCallRender("continue");
                    break;
                case "renderSettings":
                    MainRenderSettings();
                    break;
                case "addTransform":
                    MainAddTransform();
                    break;
                case "addFinal":
                    MainAddFinal();
                    break;
                case "back color":
                    MainSelectBackColor();
                    break;
                case "toPostPro":
                    ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_WINDOW_CONTENT_BY_PARAMS", "PostFlame", null);
                    break;
            }
        }

        private void MainSelectBackColor()
        {
            _uiPickMode = UiPickMode.BackColor;
            _colorPickProvider = new ColorPickProvider(CallbackColorPickProvider, BackColor.Color);
            _colorPickProvider.Exec();
        }


        private void MainSaveFlame()
        {
            CreateFileDialog(FileViewType.SaveFlame);
        }

        private void MainLoadFlame()
        {
            CreateFileDialog(FileViewType.LoadFlame);
        }

        private void MainSaveRender()
        {
            CreateFileDialog(FileViewType.SaveRender);
        }

        private void MainLoadRender()
        {
            CreateFileDialog(FileViewType.LoadRender);
        }


        private async void MainNew()
        {
            RenderMachine.MainRenderStop();
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


        private void MainRenderSettings()
        {
            var uRenderSettingsView = new RenderSettingsView();
            var vm = (RenderSettingsViewModel) uRenderSettingsView.DataContext;
            vm.Set(_renderSettings, ActionRenderSettingsCallback);
            TopContent = uRenderSettingsView;
        }

        private int MainAddFinal(bool act = true)
        {
            //todo main. add final transform
            var utv = new FinalView();
            var dc = utv.DataContex;
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
            if (act) Act("AddTransformation", id);
            return id;
        }


        private int MainAddTransform(bool act = true)
        {
            //var utv = new TransformView();
            var utv = new TransformView();
            var dc = utv.DataContex;
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
            if (act) Act("AddTransformation", id);
            return id;
        }

        #endregion

        #region bindings

        #region bindings commands

        public ICommand CommandColorRadioChecked { get; set; }
        public ICommand CommandEditGradient { get; set; }
        public ICommand MultiCommand { get; set; }

        #endregion

        #region bindings other

        //public ObservableCollection<TransformView> Transforms { get; set; }

        public ObservableCollection<IView> Transforms { get; set; }


        [ValueBind(true)]
        public bool PanelIsEnabled
        {
            get => Get();
            set => Set(value);
        }


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

        //todo: bindings buttons isEnabled
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

        public ICommand CommandPanelIsEnabledChanged { get; set; }

        #endregion

        #endregion

        #region Actions

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

        private void ActionRenderSettingsCallback()
        {
            TopContent = null;
        }

        private void ActionTransformPickColor(int id, Color color)
        {
            _uiPickMode = UiPickMode.TransformColor;
            _transformId = id;
            _colorPickProvider = new ColorPickProvider(CallbackColorPickProvider, color);
            _colorPickProvider.Exec();
        }


        private void MainActionCallRender(string renderType)
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
                    //var vm = (TransformViewModel) transformationView.DataContext;
                    var vm = transformationView.DataContex;
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
                //var dataContext = (TransformViewModel) Transforms[i].DataContext;
                var dataContext = Transforms[i].DataContex;
                variations[i] = dataContext.GetVariation;
                transforms[i] = dataContext.GetTransform;
            }

            gradientModel = null;
            if (_flameColorMode == FlameColorMode.Gradient) gradientModel = _gradientModel;
        }

        private void Act(string name, object obj)
        {
            ActRenderSetState(RenderMachine.StateRenderEnded);
            MainActionCallRender("draft");
        }

        private void ActionTransformRemove(int id)
        {
            // var transformation =
            //     Transforms.FirstOrDefault(x => ((TransformViewModel) x.DataContext).Id == id);
            var transformation =
                Transforms.FirstOrDefault(x => x.DataContex.Id == id);

            if (transformation == null) return;
            Transforms.Remove(transformation);

            ActRenderSetState(RenderMachine.StateRenderEnded);

            if (Transforms.Count > 0) MainActionCallRender("draft");
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
                    var model = JsonFlamesModel.GetFlameModelFromString(flameModelJson);
                    if (model == null) return;
                    SetEnvironmentFromFlameModel(model);
                    _flameName = Path.GetFileNameWithoutExtension(path);
                    RenderMachine.MainRenderStop();
                    ActRenderSetState(RenderMachine.StateRenderEnded);
                    MainActionCallRender("draft");
                    break;
                case FileViewType.SaveFlame:
                    RenderMachine.MainRenderStop();

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
                    var flameModel = JsonFlamesModel.GetFlameModelFromString(json);
                    SetEnvironmentFromFlameModel(flameModel);
                    _flameName = Path.GetFileNameWithoutExtension(path);
                    RenderMachine.MainRenderStop();
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
            //todo: isFinal
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
            //todo final
            GetDataForRender(out var transforms, out var variations, out var viewSettings, out var gradientModel);
            var json = JsonFlamesModel.GetFlameModelJson(transforms, variations, viewSettings, gradientModel);
            return json;
        }


        #region grad-view

        private void EditGradientHandler(object obj)
        {
            _uiPickMode = UiPickMode.EditGradient;
            _gradientPickProvider = new GradientPickProvider(CallbackGradientPickProvider, _gradientModel);
            _gradientPickProvider.Exec();
        }

        private void ActionTransformPickGradientColor(int id, double colorPosition)
        {
            _uiPickMode = UiPickMode.GradientColor;
            _transformId = id;
            _gradientPickProvider =
                new GradientPickProvider(CallbackGradientPickProvider, _gradientModel, colorPosition);
            _gradientPickProvider.Exec();
        }

        #endregion

        #endregion

        #region methods

        private void SetColorModeForAllTransforms(FlameColorMode flameColorMode)
        {
            GradientModel gm = null;
            if (_gradientModel == null) _gradientModel = new GradientModel(Colors.Gray, Colors.Gray);
            if (flameColorMode == FlameColorMode.Gradient) gm = _gradientModel;

            foreach (var transformView in Transforms)
            {
                // var dc = (TransformViewModel) transformView.DataContext;
                var dc = transformView.DataContex;
                dc.GradientModel = gm;
                dc.FlameColorMode = flameColorMode;
            }
        }


        private void CreateFileDialog(FileViewType fileViewType)
        {
            var dialog = new FileView();
            var vm = (FileViewModel) dialog.DataContext;
            vm.Set(fileViewType, ActDialogResult);
            TopContent = dialog;
        }

        private void SetTransformationColorMode(FlameColorMode flameColorMode)
        {
            GradientModel gm = null;
            if (_gradientModel == null) _gradientModel = new GradientModel(Colors.Gray, Colors.Gray);
            if (flameColorMode == FlameColorMode.Gradient) gm = _gradientModel;

            foreach (var uTransformationView in Transforms)
            {
                //var dc = (TransformViewModel) uTransformationView.DataContext;
                var dc = uTransformationView.DataContex;
                dc.GradientModel = gm;
                dc.FlameColorMode = flameColorMode;
            }
        }

        private static void InitDirectories()
        {
            var directories = Directories.GetAll;
            foreach (var directory in directories)
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
        }


        private void InitStrings()
        {
            BindStorage.SetActionFor(Act, _bindParameters1);

            ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_VERSION", 1, 1, 1);
            ActionFire.Invoke("MAIN_WINDOW_VIEWMODEL-SET_BOTTOM_STRING", "app started...");
        }


        private void LoadCoefficients(double[] coefficients, double probability, Color color, double colorPosition,
            int variationId,
            double[] parameters, bool modelIsFinal, double weight = 1.0)
        {
            var id = MainAddTransform(false);
            TransformModel model = new TransformModel();
           TransformViewModelBase tf = new TransformViewModel();
           
            if (modelIsFinal)
            {
                //todo: isFinal
                tf = new TransformFinalViewModel();
            }


            
            model.SetFromCoefficients(coefficients, probability, color, modelIsFinal, colorPosition);


            var utv = Transforms
                .FirstOrDefault(x => x.DataContex.Id == id)
                ?.DataContex;

            if (utv == null) return;

            utv.IsFinal = modelIsFinal;

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

                //todo: isFinal
                if(model.IsFinal.Count==0)
                    LoadCoefficients(c, probability, color, colorPosition, variationId, parameters, false,
                        weight);
                else
                LoadCoefficients(c, probability, color, colorPosition, variationId, parameters, model.IsFinal[i],
                    weight);
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

        #region picker providers

        private ColorPickProvider _colorPickProvider;
        private GradientPickProvider _gradientPickProvider;
        private double _providerGradientValue;
        private Color _providerColor;
        private GradientModel _providerGradientModel;
        private UiPickMode _uiPickMode;

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
                                _providerGradientModel = _gradientPickProvider.ResultGradientModel.Copy();
                                break;
                            case GradientMode.Select:
                                _providerGradientValue = _gradientPickProvider.ResultValue;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                    if (_gradientPickProvider.Result)
                        AfterProvider();
                    //UpdateUi();

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
                        _providerColor = _colorPickProvider.ResultColor;
                        AfterProvider();
                        //UpdateUi();
                    }

                    _colorPickProvider = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }

        private void AfterProvider()
        {
            TransformViewModelBase transformTransformViewModel;
            switch (_uiPickMode)
            {
                case UiPickMode.BackColor:
                    BackColor = new SolidColorBrush(_providerColor);
                    break;
                case UiPickMode.TransformColor:
                    // transformViewModel = (TransformViewModel) Transforms
                    //     .FirstOrDefault(x => ((TransformViewModel) x.DataContext).Id == _transformId)
                    //     ?.DataContext;
                    transformTransformViewModel = Transforms
                        .FirstOrDefault(x => x.DataContex.Id == _transformId)
                        ?.DataContex;

                    if (transformTransformViewModel == null) return;
                    transformTransformViewModel.FColor = _providerColor;
                    break;
                case UiPickMode.EditGradient:
                    _gradientModel = _providerGradientModel.Copy();
                    SetTransformationColorMode(_flameColorMode);
                    break;
                case UiPickMode.GradientColor:
                    transformTransformViewModel = Transforms.FirstOrDefault(x =>
                            x.DataContex.Id == _transformId)
                        ?.DataContex;
                    // transformViewModel = (TransformViewModel) Transforms.FirstOrDefault(x =>
                    //         ((TransformViewModel) x.DataContext).Id == _transformId)
                    //     ?.DataContext;

                    if (transformTransformViewModel != null)
                        transformTransformViewModel.ColorPosition = _providerGradientValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}