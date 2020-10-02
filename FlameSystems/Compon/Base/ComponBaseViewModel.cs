using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Compon.Base
{
    public class ComponBaseViewModel : Notifier, IComponentViewModel
    {
        [ValueBind] public string Value { get; set; }

        public ValueBindStorage ValueMintStorage
        {
            get => BindStorage;
            set => BindStorage = value;
        }
    }
}