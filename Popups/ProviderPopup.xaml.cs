using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.Popups;
public partial class ProviderPopup : Popup
{
    public ProviderPopup(Provider provider)
    {
        InitializeComponent();
        DataContext = provider;
    }
    public async Task<bool> Create()
    {
        SetHeader("CREAR PROVEEDOR");
        object? response = await DialogHost.Show(this, "RootDialog");
        if(response is bool b && b)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> Update()
    {
        SetHeader("EDITAR PROVEEDOR");
        object? response = await DialogHost.Show(this, "RootDialog");
        if (response is bool b && b)
        {
            return true;
        }
        return false;
    }
}
