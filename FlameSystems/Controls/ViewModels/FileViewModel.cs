using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FlameBase.Enums;
using FlameBase.Models;
using FlameBase.RenderMachine;
using FlameBase.RenderMachine.Models;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Controls.ViewModels
{
    internal class FileViewModel : Notifier
    {
        private Action<bool, FileViewType, string> _resultAction;
        //private readonly string _flamesDirectory = Directories.Flames;

        private FileViewType _viewType;

        public FileViewModel()
        {
            ListItems = new ObservableCollection<string>();

            Command = new RelayCommand(CommandHandler);

            BindStorage.SetActionFor("SelectedItem", ActionSelectedItem);
            BindStorage.SetActionFor("FileName", ActionSelectedItem);

            PreviewBorder1 = new Thickness(0);
            SelectedIndex = -1;
         
        }

        public void Set(FileViewType fileViewType, Action<bool, FileViewType, string> resultAction)
        {
            _resultAction = resultAction;
            _viewType = fileViewType;
            switch (fileViewType)
            {
                case FileViewType.LoadFlame:
                case FileViewType.LoadRender:
                    OkButtonText = "load";
                    break;
                case FileViewType.SaveFlame:
                case FileViewType.SaveRender:
                    OkButtonText = "save";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileViewType), fileViewType, null);
            }
            GetFiles();
        }

        #region actions

        private void ActionSelectedItem(string arg1, object arg2)
        {
            var name = (string) arg2;
            var isExist = CheckExist((string) arg2);
            SetPreviewAppearance(isExist);
            switch (arg1)
            {
                case "SelectedItem":
                    BindStorage.TurnActionFor("FileName", false);
                    FileName = name;
                    BindStorage.TurnActionFor("FileName", true);
                    RenderPreview(name);
                    break;
                case "FileName":
                    if (isExist)
                    {
                        BindStorage.TurnActionFor("SelectedItem", false);
                        SelectedIndex = ListItems.IndexOf(name);
                        BindStorage.TurnActionFor("SelectedItem", true);
                        RenderPreview(name);
                    }

                    break;
            }
        }

        private void ActImage(BitmapSource img)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { FlamePreview = img; });
            }
            catch (TaskCanceledException)
            {
            }
            catch (NullReferenceException)
            {
            }
        }

        #endregion

        #region bindings

        public ObservableCollection<string> ListItems { get; set; }

        [ValueBind]
        public BitmapSource FlamePreview
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string FileName
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string FlameInfo
        {
            get => Get();
            set => Set(value);
        }

        public ICommand Command { get; }

        [ValueBind]
        public string OkButtonText
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string SelectedItem
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public int SelectedIndex
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Thickness PreviewBorder1
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Thickness PreviewBorder2
        {
            get => Get();
            set => Set(value);
        }

        #endregion

        #region private

        private void SetPreviewAppearance(bool appearance)
        {
            switch (appearance)
            {
                case true:
                    PreviewBorder1 = new Thickness(1, 1, 1, 0);
                    PreviewBorder2 = new Thickness(1);
                    break;
                default:
                    PreviewBorder1 = new Thickness(0);
                    PreviewBorder2 = new Thickness(0);
                    FlamePreview = null;
                    FlameInfo = null;
                    RenderMachine.MainRenderStop();
                    break;
            }
        }


        private string GetJsonPath(string filename)
        {
            string jsonPath;

            switch (_viewType)
            {
                case FileViewType.LoadFlame:
                case FileViewType.SaveFlame:
                    jsonPath = $"{Directories.Flames}\\{filename}.fJson";
                    break;
                case FileViewType.SaveRender:
                case FileViewType.LoadRender:
                    jsonPath = $"{Directories.Renders}\\{filename}.gz";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return jsonPath;
        }

        private void CommandHandler(object obj)
        {
            //var jsonPath = $"{_flamesDirectory}\\{FileName}.fJson";
            var jsonPath = GetJsonPath(FileName);

            switch ((string) obj)
            {
                case "cancel":
                    _resultAction(false, _viewType, string.Empty);
                    break;
                case "save":
                    if (!CheckIsValid(FileName)) break;
                    RenderMachine.MainRenderStop();
                    _resultAction(true, _viewType, jsonPath);
                    break;
                case "load":
                    if (!CheckIsValid(FileName)) break;
                    if (!CheckExist(jsonPath)) break;
                    RenderMachine.MainRenderStop();
                    _resultAction(true, _viewType, jsonPath);
                    break;
            }
        }

        private static bool CheckIsValid(string name)
        {
            return !string.IsNullOrEmpty(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        private static bool CheckExist(string path)
        {
            return !string.IsNullOrEmpty(path) && File.Exists(path);
        }


        private void RenderPreview(string name)
        {
            var jsonPath = GetJsonPath(name);

            var jsonString = File.ReadAllText(jsonPath);
            FlameModel model;
            try
            {
                model = JsonFlamesModel.GetFlameModel(jsonString);
                if (model == null) return;
            }
            catch (Exception)
            {
                return;
            }

            FlameInfo = GetInfo(model);

            const int width = 200;
            var aspect = 1.0 * model.ImageHeight / model.ImageWidth;
            var viewSettings = new ViewSettingsModel(width, (int) (aspect * width), model.ViewShiftX,
                model.ViewShiftY, model.ViewZoom, model.Rotation, model.Symmetry, model.BackColor);
            var variations = FlameHelperModel.GetVariationsFromFlameModel(model);
            var transformations = FlameHelperModel.GetTransformationsFromFlameModel(model);
            var renderSettings = new RenderSettingsModel(50, 10);

            GradientModel gradModel = null;
            var colorMode = FlameColorMode.Color;
            if (model.GradientPack != null)
            {
                colorMode = FlameColorMode.Gradient;
                gradModel = new GradientModel(model.GradientPack);
            }

            var renderPack = new RenderPackModel(transformations, variations, viewSettings, renderSettings,
                colorMode, gradModel);
            var renderActions = new RenderActionsModel(ActImage, null, null);
            RenderMachine.Render(renderPack, renderActions);
        }

        private static string GetInfo(FlameModel model)
        {
            var str = string.Empty;
            str += $"image width: {model.ImageWidth}\n";
            str += $"image height: {model.ImageHeight}";
            return str;
        }


        private void GetFiles()
        {

            string flamesDirectory;
            string extension;
            switch (_viewType)
            {
                case FileViewType.LoadFlame:
                case FileViewType.SaveFlame:
                    flamesDirectory = Directories.Flames;
                    extension = ".fJson";
                    break;
                case FileViewType.SaveRender:
                case FileViewType.LoadRender:
                    flamesDirectory = Directories.Renders;
                    extension = ".gz";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var files = Directory.GetFiles(flamesDirectory);
            foreach (var file in files)
            {
                if (!string.Equals(Path.GetExtension(file), extension, StringComparison.CurrentCultureIgnoreCase)) continue;
                var name = Path.GetFileNameWithoutExtension(file);
                ListItems.Add(name);
            }
        }

        #endregion
    }
}