using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.Popups;

/// <summary>
/// Lógica de interacción para SellPopup.xaml
/// </summary>
public partial class SellPopup : Popup
{
    public SellPopup(Sell vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    public async Task View()
    {
        SetHeader("VENTA");
        await DialogHost.Show(this, "RootDialog");
    }
}
