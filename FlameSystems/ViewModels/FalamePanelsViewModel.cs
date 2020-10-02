using FlameSystems.Compon.Base;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.ViewModels
{
    public class FalamePanelsViewModel : Notifier, IComponentViewModel
    {
        public ValueBindStorage ValueMintStorage
        {
            get => BindStorage;
            set => BindStorage = value;
        }
    }
}