using System;
using System.Windows.Controls;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;
using FlameSystems.Views;

namespace FlameSystems.ViewModels
{
    internal class MainWindowViewModel : Notifier
    {
        private const string ProgramName = "Flame Systems";

        public MainWindowViewModel()
        {
            var thisType = GetType();
            ActionFire.AddOrReplace("MAIN_WINDOW_VIEWMODEL-SET_VERSION",
                new Action<int, int, int>(ActionSetVersionString), thisType);
            ActionFire.AddOrReplace("MAIN_WINDOW_VIEWMODEL-SET_BOTTOM_STRING",
                new Action<string>(ActionSetBottomString), thisType);
            ActionFire.AddOrReplace("MAIN_WINDOW_VIEWMODEL-SET_WINDOW_CONTENT",
                new Action<Control>(ActionSetWindowContent), thisType);
            ActionFire.AddOrReplace("MAIN_WINDOW_VIEWMODEL-SET_WINDOW_CONTENT_BY_PARAMS",
                new Action<string, object>(ActionSetWindowContentByParams), thisType);

            WindowContent = new CreateFlameView();
        }

        #region bindings

        [ValueBind]
        public Control WindowContent
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string BottomString
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string UpperString
        {
            get => Get();
            set => Set(value);
        }

        #endregion

        #region actions

        private uint _t;

        private void ActionSetBottomString(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            BottomString = $"({_t++:00000000}) {text}";
        }

        private void ActionSetVersionString(int v1, int v2, int v3)
        {
            UpperString = $"{ProgramName} v{v1}.{v2}.{v3}";
        }

        private void ActionSetWindowContent(Control control)
        {
            if (control == null) return;
            WindowContent = control;
        }

        private void ActionSetWindowContentByParams(string contentName, object parameter)
        {
            switch (contentName)
            {
                case "CreateFlame":
                    WindowContent = new CreateFlameView();
                    break;
                case "PostFlame":
                    WindowContent = new PostFlameView();
                    break;
            }
        }

        #endregion
    }
}