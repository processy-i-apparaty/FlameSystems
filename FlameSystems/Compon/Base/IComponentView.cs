using System.Windows.Controls;

namespace FlameSystems.Compon.Base
{
    public interface IComponentView
    {
        IComponentViewModel ViewModel { get; set; }
        ContentControl ComtempControl { get; set; }
    }
}