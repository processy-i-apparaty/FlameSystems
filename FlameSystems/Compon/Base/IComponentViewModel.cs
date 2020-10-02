using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Compon.Base
{
    public interface IComponentViewModel
    {
        ValueBindStorage ValueMintStorage { get; set; }
    }
}