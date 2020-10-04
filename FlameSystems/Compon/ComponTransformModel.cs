using System;
using System.Collections.Generic;
using FlameSystems.Compon.Base;

namespace FlameSystems.Compon
{
    public class ComponTransformModel : ComponModel
    {
        public ComponTransformModel(List<string> valueNames, List<Type> valueTypes, IComponentView view) : base(
            valueNames, valueTypes, view)
        {
        }
    }
}