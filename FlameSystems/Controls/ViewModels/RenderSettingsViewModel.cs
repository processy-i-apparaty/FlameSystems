using System;
using System.Windows;
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
            BindStorage.SetActionFor("RenderByQuality", ActionRenderByQuality);

            _renderSettings = renderSettings;
            ShotsPerIteration = _renderSettings.ShotsPerIteration * 0.001;
            Iterations = _renderSettings.Iterations * 0.001;
            RenderPerIterations = _renderSettings.RenderPerIterations;
            RenderColorMode = new RenderColorModeModel();
            RenderColorMode.SetMode(_renderSettings.RenderColorMode);
            Quality = renderSettings.Quality;
            if (renderSettings.RenderByQuality)
            {
                RenderByQuality = true;
            }
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
                    _renderSettings.RenderByQuality = RenderByQuality;
                    _renderSettings.Quality = Quality;
                    _callback();
                    break;
                case "cancel":
                    Result = false;
                    _callback();
                    break;
            }
        }


        private void ActionRenderByQuality(string arg1, object arg2)
        {
            var val = (bool) arg2;
            if (val)
            {
                VisibilityQuality = Visibility.Visible;
                VisibilityIterations = Visibility.Collapsed;
            }
            else
            {
                VisibilityQuality = Visibility.Collapsed;
                VisibilityIterations = Visibility.Visible;
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

        [ValueBind]
        public bool RenderByQuality
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(Visibility.Collapsed)]
        public Visibility VisibilityQuality
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(10.0)]
        public double Quality
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(Visibility.Visible)]
        public Visibility VisibilityIterations
        {
            get => Get();
            set => Set(value);
        }

        #endregion
    }
}