using System;
using System.Collections.Generic;
using System.Windows.Documents;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Connectors.Base
{
    public class ConnectorPanelTransforms : IConnector
    {
        public List<IValue> Values { get; set; }
        public List<IAction> Actions { get; set; }
        public IControlV ControlV { get; set; }
        public IControlVm ControlVm { get; set; }

        public void SetVal(IValue value)
        {
            ControlVm.MintStorage.SetIValue(value);
        }

        public void SetAct(IAction action)
        {
            ControlVm.MintStorage.SetAction(action);
        }

        public IValue GetVal(string name)
        {
            return ControlVm.MintStorage.GetValue(name);
        }

        public IAction GetAct(string name)
        {
            return ControlVm.MintStorage.GetAction(name);
        }

        [ValueBind] public PanelModel PanelModel { get; set; } = new PanelModel();

        public ConnectorPanelTransforms(List<IValue> values, List<IAction> actions, IControlV control)
        {
            Values = values;
            Actions = actions;
            ControlV = control;
            ControlVm = ControlV.ViewModel;
        }
    }
}