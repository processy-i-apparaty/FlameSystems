using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using FlameBase.Enums;
using FlameBase.Models;
using FlameBase.RenderMachine;
using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views;

namespace FlameSystems.Infrastructure
{
    internal class LoaderSaverProvider : Notifier
    {
        private readonly Action<string, string, Control> _providerCallback;
        private readonly uint[,,] _display;
        private readonly FileViewType _fileViewType;
        private readonly object _model;

        public LoaderSaverProvider(FileViewType fileViewType, Action<string, string, Control> providerCallbackAction,
            object model = null,
            uint[,,] display = null)
        {
            _fileViewType = fileViewType;
            _model = model;
            _display = display;
            _providerCallback = providerCallbackAction;
            Exec();
        }

        // public Control Content { get; private set; }
        public bool Result { get; private set; }
        public string ResultPath { get; private set; }
        public string ResultString { get; private set; }
        public string FlameName { get; private set; }
        public FlameModel Flame { get; private set; }
        public uint[,,] Display { get; private set; }

        public bool Busy { get; private set; }
        // public bool IsDialogOn { get; private set; }

        private void Exec()
        {
            Busy = true;
            var dialog = new FileView();
            var vm = (FileViewModel) dialog.DataContext;
            vm.Set(_fileViewType, CallbackAction);
            _providerCallback.Invoke("dialog", "", dialog);
        }

        private async void CallbackAction(bool result, FileViewType fileViewType, string resultPath)
        {
            Result = result;
            ResultPath = resultPath;
            if (!result)
            {
                Busy = false;
                ResultString = "Operation canceled";
                _providerCallback.Invoke("end", "", null);
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
                    _providerCallback.Invoke("spinner", $"saving render {FlameName}...", null);

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
                    _providerCallback.Invoke("end", "", null);
                    return;
                }
            }
            catch (Exception e)
            {
                Result = false;
                ResultString = $"SaveRender exception: {e.Message}";
                Busy = false;
                _providerCallback.Invoke("end", "", null);
                return;
            }

            Result = true;
            ResultString = $"Render saved successfully with name: {FlameName}";
            _providerCallback.Invoke("end", "", null);
            Busy = false;
        }

        private void LoadRender()
        {
            if (!CheckFileExist()) return;

            try
            {
                FlameName = Path.GetFileNameWithoutExtension(ResultPath);
                _providerCallback.Invoke("spinner", $"loading render {FlameName}...", null);

                var tempDir = CreateTempDir(FlameName);
                ZipFlamesModel.DecompressToDirectory(ResultPath, tempDir);

                Display = (uint[,,]) BinaryFlamesModel.LoadObject($"{tempDir}\\logDisplay.bin");
                var json = File.ReadAllText($"{tempDir}\\flame.txt");
                Flame = JsonFlamesModel.GetFlameModel(json);
                RenderMachine.LoadDisplay(Display, Flame.BackColor);

                RemoveTempDir();
            }
            catch (Exception e)
            {
                Result = false;
                ResultString = $"SaveRender exception: {e.Message}";
                Busy = false;
                _providerCallback.Invoke("end", "", null);
                return;
            }

            Result = true;
            ResultString = $"Render {FlameName} loaded successfully";
            _providerCallback.Invoke("end", "", null);
            Busy = false;
        }

        private void SaveFlame()
        {
            try
            {
                if (_model is string str && ResultPath != null)
                {
                    FlameName = Path.GetFileNameWithoutExtension(ResultPath);
                    _providerCallback.Invoke("spinner", $"saving flame {FlameName}...", null);

                    File.WriteAllText(ResultPath, str);
                }
                else
                {
                    Result = false;
                    ResultString = "Model is not a json string";
                    Busy = false;
                    _providerCallback.Invoke("end", "", null);
                    return;
                }
            }
            catch (Exception e)
            {
                Result = false;
                ResultString = $"SaveFlame exception: {e.Message}";
                Busy = false;
                _providerCallback.Invoke("end", "", null);
                return;
            }

            Result = true;
            ResultString = $"Flame saved successfully with name: {FlameName}";
            _providerCallback.Invoke("end", "", null);
            Busy = false;
        }

        private void LoadFlame()
        {
            if (!CheckFileExist()) return;


            try
            {
                FlameName = Path.GetFileNameWithoutExtension(ResultPath)
                            ?? throw new ArgumentNullException(nameof(ResultPath), @"ResultPath is null");

                _providerCallback.Invoke("spinner", $"loading flame {FlameName}...", null);

                var flameModelJson = File.ReadAllText(ResultPath);
                Flame = JsonFlamesModel.GetFlameModel(flameModelJson);

                if (Flame == null)
                {
                    Result = false;
                    ResultString = "Flame read error";
                    _providerCallback.Invoke("end", "", null);
                    Busy = false;
                    return;
                }
            }
            catch (Exception e)
            {
                Result = false;
                ResultString = $"LoadFlame exception: {e.Message}";
                Busy = false;
                _providerCallback.Invoke("end", "", null);
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
            _providerCallback.Invoke("end", "", null);
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