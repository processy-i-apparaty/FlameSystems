using System.Collections.Generic;

namespace FlameSystems.Connectors.Base
{
    public class BaseAction : IAction
    {
        public BaseAction(string nameOf, List<IValue> listArguments)
        {
            NameOf = nameOf;
            ListArguments = listArguments;
        }

        public BaseAction()
        {
            NameOf = null;
            ListArguments = null;
            ValueReturn = null;
        }

        public string NameOf { get; set; }
        public List<IValue> ListArguments { get; set; }
        public IValue ValueReturn { get; set; }
    }
}