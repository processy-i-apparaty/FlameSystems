using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Connectors.Base
{
    public class PanelTransformsViewModel : Notifier, IControlVm
    {
        public PanelTransformsViewModel()
        {
            Command = new RelayCommand(Handler);
            Transforms = new ObservableCollection<PanelTransform>();
            ActionFire.AddOrReplace("panel-transform-remove", new Action<int>(HandlerTransformRemove), GetType());
            PanelModel = new PanelModel();
            BindStorage.SetActionFor(HandlerChanged, "Transforms", "Final");
        }

        [ValueBind]
        public ObservableCollection<PanelTransform> Transforms
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public PanelTransform Final
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public PanelModel PanelModel
        {
            get => Get();
            set => Set(value);
        }

        public ICommand Command { get; }

        public ValueBindStorage MintStorage
        {
            get => BindStorage;
            set => BindStorage = value;
        }

        private void HandlerChanged(string arg1, object arg2)
        {
            PanelModel = new PanelModel(Transforms, Final);
        }

        private void HandlerTransformRemove(int id)
        {
            var item = Transforms.FirstOrDefault(x => ((PanelTransformViewModel) x.DataContext).Id == id);
            if (Final != null)
            {
                var final = (PanelTransformViewModel) Final.DataContext;
                if (final.Id == id)
                    Final = null;
                //call update
            }
            else
            {
                if (item != null)
                    Transforms.Remove(item);
                //call update
            }
        }

        private void Handler(object obj)
        {
            switch ((string) obj)
            {
                case "add transform":
                    Transforms.Add(Factory.GetTransformView);
                    break;
                case "add final":
                    Final = Factory.GetFinalView;
                    break;
            }
        }
    }

    public static class Factory
    {
        public static PanelTransform GetTransformView
        {
            get
            {
                var view = new PanelTransform();
                return view;
            }
        }

        public static PanelTransform GetFinalView
        {
            get
            {
                var view = new PanelTransform();
                var dc = (PanelTransformViewModel) view.DataContext;
                // dc.VisibilityColorSelector = Visibility.Hidden;
                // dc.Text = "__final__";
                return view;
            }
        }
    }
}