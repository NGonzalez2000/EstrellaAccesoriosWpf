using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.Popups;

/// <summary>
/// Lógica de interacción para TotalPopup.xaml
/// </summary>
public partial class TotalPopup : Popup
{
    public TotalPopup(Sell sell)
    {
        InitializeComponent();
        DataContext = sell;
    }
    public async Task<bool> Show()
    {
        SetHeader("TOTAL COBRADO");
        TB_Money.Focus();
        object? response = await DialogHost.Show(this, "SellDialog");
        if (response is bool b && b)
        {
            return true;
        }
        return false;
    }
}
