using System;
using System.Collections.Generic;

namespace ComponentApp.Compon.Base
{
    public abstract class ComponBaseModel
    {
        protected ComponBaseModel(
            List<string> valueNames,
            List<Type> valueTypes,
            IComponentView view
        )
        {
            ValueNames = valueNames;
            ValueTypes = valueTypes;
            View = view;
            ViewModel = View.ViewModel;
        }

        public IComponentViewModel ViewModel { get; set; }
        public IComponentView View { get; set; }
        public List<Type> ValueTypes { get; set; }
        public List<string> ValueNames { get; set; }
        public int TotalValues => ValueNames.Count;

        public void ValueSet(ValueModel value)
        {
            ViewModel.ValueMintStorage.Set(value.Name, value.Data, value.TypeOf);
        }

        public ValueModel ValueGet(string name)
        {
            var get = ViewModel.ValueMintStorage.Get(name);
            var typ = ViewModel.ValueMintStorage.Type(name);
            return new ValueModel(name, get, typ);
        }

        public void ValueSetAll(List<ValueModel> values)
        {
            for (var i = 0; i < values.Count; i++)
                ValueSet(new ValueModel(values[i].Name, values[i].Data, values[i].TypeOf));
        }

        public List<ValueModel> ValueGetAll()
        {
            var list = new List<ValueModel>();
            foreach (var name in ValueNames)
                list.Add(ValueGet(name));
            return list;
        }
    }
}