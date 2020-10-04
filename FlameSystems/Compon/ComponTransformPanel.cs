using System.Collections.Generic;
using FlameSystems.Compon.Base;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Compon
{
    public class ComponTransformPanel : ComponBaseModel
    {
        [ValueBind] public List<IComponentView> Transforms { get; set; }
        [ValueBind] public IComponentView FinalTransform { get; set; }
    }
}