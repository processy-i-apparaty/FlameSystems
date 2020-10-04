using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FlameSystems.Compon;
using FlameSystems.Compon.Base;
using FlameSystems.Connectors;
using FlameSystems.Connectors.Base;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.ViewModels
{
    public class FalamePanelsViewModel : Notifier, IComponentViewModel
    {
        private ConnectorPanelTransforms _connectorPanelTransforms;

        public ValueBindStorage ValueMintStorage
        {
            get => BindStorage;
            set => BindStorage = value;
        }

        public ICommand CommandLoaded { get; }

        public ICommand CommandPush
        {
            get;
        }

        public FalamePanelsViewModel()
        {
            CommandLoaded = new RelayCommand(HandlerLoaded);
            CommandPush = new RelayCommand(HandlerPush);
        }

        private void HandlerPush(object obj)
        {
            var val = _connectorPanelTransforms.GetVal("PanelModel");
            var panelModel = (PanelModel) val.ObjectOf;

        }

        private void HandlerLoaded(object obj)
        {
            //todo compon
            var control = (ContentControl) obj;
            switch (control.Name)
            {
                case "ComponentPanel1":
                    break;
                case "ComponentPanel2":
                    break;
                case "ComponentDisplay":
                    break;
                case "ComponentTransforms":
                    // var names = new List<string> {"parameters"};
                    // var types = new List<Type> {typeof(List<ComponTransformParameter>)};
                    //
                    // var ctp = new ComponTransformPanel();
                    //
                    // var componModel = new ComponTransformModel(names, types, ctp);
                    // control.Content = componModel.View;
                    
                    // var componModel = ComponHelper.ComponentStandartTransform();
                    // componModel.ValueSet(new ValueModel("ShiftX", .5, typeof(double)));
                    // control.Content = componModel.View;

                    var v = new BaseValue("PanelModel", typeof(PanelModel), new PanelModel());
                    _connectorPanelTransforms = new ConnectorPanelTransforms(new List<IValue> {v}, new List<IAction>(), new PanelTranforms());
                    control.Content = (Control) _connectorPanelTransforms.ControlV;
                    
                    break;
            }
        }
    }
}