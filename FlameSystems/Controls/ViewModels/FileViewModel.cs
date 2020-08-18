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
        private readonly string _flamesDirectory = Directories.Flames;
        private FileViewType _viewType;
        private Action<bool, FileViewType, string> _resultAction;

        public FileViewModel()
        {
            ListItems = new ObservableCollection<string>();

            Command = new RelayCommand(CommandHandler);

            BindStorage.SetActionFor("SelectedItem", ActionSelectedItem);
            BindStorage.SetActionFor("FileName", ActionSelectedItem);

            PreviewBorder1 = new Thickness(0);
            SelectedIndex = -1;
            GetFiles();
        }

        public void Set(FileViewType fileViewType, Action<bool, FileViewType, string> resultAction)
        {
            _resultAction = resultAction;
            _viewType = fileViewType;
            switch (fileViewType)
            {
                case FileViewType.Load:
                    OkButtonText = "load";
                    break;
                case FileViewType.Save:
                    OkButtonText = "save";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileViewType), fileViewType, null);
            }
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
                    RenderMachine.RenderStop();
                    break;
            }
        }

        private void CommandHandler(object obj)
        {
            var jsonPath = $"{_flamesDirectory}\\{FileName}.fJson";
            switch ((string) obj)
            {
                case "cancel":
                    _resultAction(false, _viewType, string.Empty);
                    break;
                case "save":
                    if (!CheckIsValid(FileName)) break;
                    RenderMachine.RenderStop();
                    _resultAction(true, _viewType, jsonPath);
                    break;
                case "load":
                    if (!CheckIsValid(FileName)) break;
                    if (!CheckExist(FileName)) break;
                    RenderMachine.RenderStop();
                    _resultAction(true, _viewType, jsonPath);
                    break;
            }
        }

        private static bool CheckIsValid(string name)
        {
            return !string.IsNullOrEmpty(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        private bool CheckExist(string name)
        {
            return !string.IsNullOrEmpty(name) && File.Exists($"{_flamesDirectory}\\{name}.fJson");
        }


        private void RenderPreview(string name)
        {
            var jsonPath = $"{_flamesDirectory}\\{name}.fJson";
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
                model.ViewShiftY, model.ViewZoom, model.Rotation, model.Symmetry);
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
            var files = Directory.GetFiles(_flamesDirectory);
            foreach (var file in files)
            {
                if (Path.GetExtension(file) != ".fJson") continue;
                var name = Path.GetFileNameWithoutExtension(file);
                ListItems.Add(name);
            }
        }

        #endregion
    }
}