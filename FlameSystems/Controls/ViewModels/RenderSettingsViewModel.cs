using System;
using System.Windows.Input;
using FlameBase.RenderMachine.Models;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Controls.ViewModels
{
    internal class RenderSettingsViewModel : Notifier
    {
        private Action _callback;
        private RenderSettingsModel _renderSettings;

        public RenderSettingsViewModel()
        {
            Command = new RelayCommand(CommandHandler);
        }

        public void Set(RenderSettingsModel renderSettings, Action callback)
        {
            _renderSettings = renderSettings;
            ShotsPerIteration = _renderSettings.ShotsPerIteration * 0.001;
            Iterations = _renderSettings.Iterations * 0.001;
            RenderPerIterations = _renderSettings.RenderPerIterations;
            RenderColorMode = new RenderColorModeModel();
            RenderColorMode.SetMode(_renderSettings.RenderColorMode);
            _callback = callback;
        }

        private void CommandHandler(object obj)
        {
            switch (obj)
            {
                case "ok":
                    Result = true;
                    _renderSettings.ShotsPerIteration = (int) (ShotsPerIteration * 1000.0);
                    _renderSettings.Iterations = (int) (Iterations * 1000.0);
                    _renderSettings.RenderPerIterations = RenderPerIterations;
                    _renderSettings.RenderColorMode = RenderColorMode.Mode;
                    _callback();
                    break;
                case "cancel":
                    Result = false;
                    _callback();
                    break;
            }
        }


        #region bindings

        [ValueBind(1.0, 0.001, 1000.0)]
        public double Iterations
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(100, 50, 10000)]
        public int RenderPerIterations
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(64.0, 0.001, 1000.0)]
        public double ShotsPerIteration
        {
            get => Get();
            set => Set(value);
        }

        public bool Result { get; set; }

        public ICommand Command { get; }

        [ValueBind]
        public RenderColorModeModel RenderColorMode
        {
            get => Get();
            set => Set(value);
        }

        #endregion
    }
}