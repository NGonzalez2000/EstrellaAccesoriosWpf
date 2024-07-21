using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Popups;

/// <summary>
/// Lógica de interacción para ProductPopup.xaml
/// </summary>
public partial class ProductPopup : Popup
{
    private readonly List<Category> categories;
    private List<SubCategory> subCategories;
    private readonly List<Provider> providers;
    public ProductPopup(Product product, EstrellaDbContext dbContext)
    {
        InitializeComponent();
        DataContext = product;

        categories = [.. dbContext.Categories.Include(c => c.SubCategories)];
        subCategories = [];
        providers = [.. dbContext.Providers];

        cb_Category.ItemsSource = categories;
        cb_Provider.ItemsSource = providers;

        cb_Category.SelectedItem = product.Category;
        cb_Provider.SelectedItem = product.Provider;
    }
    public async Task<bool> Create()
    {
        SetHeader("CREAR PRODUCTO");
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

    private void Cb_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox cb) return;
        if (cb.SelectedItem is not Category selectedCategory) throw new ArgumentNullException(nameof(sender));
        subCategories = [.. selectedCategory.SubCategories];
        cb_SubCategory.ItemsSource = subCategories;
        if (subCategories.Count > 0)
        {
            int indx = subCategories.IndexOf(subCategories.First(sc => sc.Description == "SIN SUBCATEGORIA"));
            cb_SubCategory.SelectedIndex = indx;
        }
    }
}
