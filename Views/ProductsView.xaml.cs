using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;

namespace EstrellaAccesoriosWpf.Views;

/// <summary>
/// Lógica de interacción para ProductsView.xaml
/// </summary>
public partial class ProductView : View
{
    public ProductView(ProductViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}
