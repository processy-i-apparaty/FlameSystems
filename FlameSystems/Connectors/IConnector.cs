using System;
using System.Collections.Generic;
using System.Windows.Controls;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Connectors
{
    public interface IConnector
    {
        List<IValue> Values { get; set; }
        List<IAction> Actions { get; set; }
        IControlV ControlV { get; set; } //View -> Content
        IControlVm ControlVm { get; set; }
        void SetVal(IValue value);
        void SetAct(IAction action);
        IValue GetVal(string name);
        IAction GetAct(string name);
    }

    public interface IValue
    {
        string NameOf { get; set; }
        object ObjectOf { get; set; }
        Type TypeOf { get; set; }
    }

    public interface IControlV
    {
        IControlVm ViewModel { get; set; }
    }

    public interface IControlVm
    {
        ValueBindStorage MintStorage { get; set; }
    }

    public interface IAction
    {
        string NameOf { get; set; }
        List<IValue> ListArguments { get; set; }
        IValue ValueReturn { get; set; }
    }
}