using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Popups;

/// <summary>
/// Lógica de interacción para CategoryPopup.xaml
/// </summary>
public partial class CategoryPopup : Popup
{
    public CategoryPopup(Category category)
    {
        InitializeComponent();
        DataContext = category;
    }
    public async Task<bool> Create()
    {
        SetHeader("CREAR CATEGORIA");
        object? response = await DialogHost.Show(this, "RootDialog");
        if (response is bool b && b)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> Update()
    {
        SetHeader("EDITAR CATEGORIA");
        object? response = await DialogHost.Show(this, "RootDialog");
        if (response is bool b && b)
        {
            return true;
        }
        return false;
    }
}
