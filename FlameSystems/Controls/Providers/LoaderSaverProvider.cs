using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using FlameBase.Enums;
using FlameBase.Models;
using FlameSystems.Controls.Pickers.Enums;
using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views;
using FlameSystems.Infrastructure;

namespace FlameSystems.Controls.Providers
{
    internal class LoaderSaverProvider : Notifier
    {
        private readonly uint[,,] _display;
        private readonly FileViewType _fileViewType;
        private readonly object _model;
        private readonly Action<ProviderCallbackType, string> _providerCallback;
        private FileView _fileView;

        public LoaderSaverProvider(FileViewType fileViewType,
            Action<ProviderCallbackType, string> providerCallback, object model = null, uint[,,] display = null)
        {
            _fileViewType = fileViewType;
            _model = model;
            _display = display;
            _providerCallback = providerCallback;
        }

        public Control ShowControl => _fileView;
        public bool Result { get; private set; }
        public string ResultPath { get; private set; }
        public string ResultString { get; private set; }
        public string FlameName { get; private set; }
        public FlameModel Flame { get; private set; }
        public uint[,,] DisplayArray { get; private set; }

        public bool Busy { get; private set; }
        // public bool IsDialogOn { get; private set; }

        public void Exec()
        {
            Busy = true;
            _fileView = new FileView();
            var vm = (FileViewModel) _fileView.DataContext;
            vm.Set(_fileViewType, CallbackAction);
            _providerCallback.Invoke(ProviderCallbackType.ShowControl, string.Empty);
        }

        private async void CallbackAction(bool result, FileViewType fileViewType, string resultPath)
        {
            Result = result;
            ResultPath = resultPath;
            if (!result)
            {
                Busy = false;
                ResultString = "Operation canceled";
                _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                return;
            }

            await Task.Run(LoadSave);
        }

        private void LoadSave()
        {
            switch (_fileViewType)
            {
                case FileViewType.LoadFlame:
                    LoadFlame();
                    break;
                case FileViewType.SaveFlame:
                    SaveFlame();
                    break;
                case FileViewType.SaveRender:
                    SaveRender();
                    break;
                case FileViewType.LoadRender:
                    LoadRender();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SaveRender()
        {
            try
            {
                if (_model is string flameModelJson && _display != null)
                {
                    FlameName = Path.GetFileNameWithoutExtension(ResultPath);
                    _providerCallback.Invoke(ProviderCallbackType.ShowSpinner, $"saving render {FlameName}...");

                    var tempDir = CreateTempDir(FlameName);
                    BinaryFlamesModel.SaveObject(_display, $"{tempDir}\\logDisplay.bin");
                    File.WriteAllText($"{tempDir}\\flame.txt", flameModelJson);
                    ZipFlamesModel.CompressDirectory(tempDir, ResultPath);
                    RemoveTempDir();
                }
                else
                {
                    Result = false;
                    ResultString = "Model is not a json string or display is null";
                    Busy = false;
                    _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                    return;
                }
            }
            catch (Exception e)
            {
                Result = false;
                ResultString = $"SaveRender exception: {e.Message}";
                Busy = false;
                _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                return;
            }

            Result = true;
            ResultString = $"Render saved successfully with name: {FlameName}";
            _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
            Busy = false;
        }

        private void LoadRender()
        {
            if (!CheckFileExist()) return;

            try
            {
                FlameName = Path.GetFileNameWithoutExtension(ResultPath);
                _providerCallback.Invoke(ProviderCallbackType.ShowSpinner, $"loading render {FlameName}...");

                var tempDir = CreateTempDir(FlameName);
                ZipFlamesModel.DecompressToDirectory(ResultPath, tempDir);

                DisplayArray = (uint[,,]) BinaryFlamesModel.LoadObject($"{tempDir}\\logDisplay.bin");
                var json = File.ReadAllText($"{tempDir}\\flame.txt");
                Flame = JsonFlamesModel.GetFlameModel(json);
                // RenderMachine.LoadDisplay(Display, Flame.BackColor);

                RemoveTempDir();
            }
            catch (Exception e)
            {
                Result = false;
                ResultString = $"SaveRender exception: {e.Message}";
                Busy = false;
                _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                return;
            }

            Result = true;
            ResultString = $"Render {FlameName} loaded successfully";
            _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
            Busy = false;
        }

        private void SaveFlame()
        {
            try
            {
                if (_model is string str && ResultPath != null)
                {
                    FlameName = Path.GetFileNameWithoutExtension(ResultPath);
                    _providerCallback.Invoke(ProviderCallbackType.ShowSpinner, $"saving flame {FlameName}...");

                    File.WriteAllText(ResultPath, str);
                }
                else
                {
                    Result = false;
                    ResultString = "Model is not a json string";
                    Busy = false;
                    _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                    return;
                }
            }
            catch (Exception e)
            {
                Result = false;
                ResultString = $"SaveFlame exception: {e.Message}";
                Busy = false;
                _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                return;
            }

            Result = true;
            ResultString = $"Flame saved successfully with name: {FlameName}";
            _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
            Busy = false;
        }

        private void LoadFlame()
        {
            if (!CheckFileExist()) return;


            try
            {
                FlameName = Path.GetFileNameWithoutExtension(ResultPath)
                            ?? throw new ArgumentNullException(nameof(ResultPath), @"ResultPath is null");

                _providerCallback.Invoke(ProviderCallbackType.ShowControl, $"loading flame {FlameName}...");

                var flameModelJson = File.ReadAllText(ResultPath);
                Flame = JsonFlamesModel.GetFlameModel(flameModelJson);

                if (Flame == null)
                {
                    Result = false;
                    ResultString = "Flame read error";
                    _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                    Busy = false;
                    return;
                }
            }
            catch (Exception e)
            {
                Result = false;
                ResultString = $"LoadFlame exception: {e.Message}";
                Busy = false;
                _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
                return;
            }

            Result = true;
            ResultString = $"Render saved successfully with name: {FlameName}";
            Busy = false;
        }

        private bool CheckFileExist()
        {
            if (File.Exists(ResultPath)) return true;
            ResultString = "File not exist";
            Result = false;
            Busy = false;
            _providerCallback.Invoke(ProviderCallbackType.End, string.Empty);
            return false;
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
    }
}