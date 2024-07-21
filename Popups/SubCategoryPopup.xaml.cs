using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.Popups;

/// <summary>
/// Lógica de interacción para SubCategoryPopup.xaml
/// </summary>
public partial class SubCategoryPopup : Popup
{
    public SubCategoryPopup(SubCategory subCategory)
    {
        InitializeComponent();
        DataContext = subCategory;
    }
    public async Task<bool> Create()
    {
        SetHeader("CREAR SUBCATEGORIA");
        object? response = await DialogHost.Show(this, "RootDialog");
        if (response is bool b && b)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> Update()
    {
        SetHeader("EDITAR SUBCATEGORIA");
        object? response = await DialogHost.Show(this, "RootDialog");
        if (response is bool b && b)
        {
            return true;
        }
        return false;
    }
}
