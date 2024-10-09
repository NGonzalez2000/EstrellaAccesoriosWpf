using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class ProductFilterViewModel : BaseFilter
{
    [ObservableProperty]
    string productCode = string.Empty;

    [ObservableProperty]
    string productDescription = string.Empty;

    [ObservableProperty]
    string productBarcode = string.Empty;

    [ObservableProperty]
    List<Category> categories;

    [ObservableProperty]
    Category? selectedCategory;

    [ObservableProperty]
    List<SubCategory> subCategories;

    [ObservableProperty]
    SubCategory? selectedSubCategory;

    [ObservableProperty]
    List<Provider> providers;

    [ObservableProperty]
    Provider? selectedProvider;
    public ProductFilterViewModel(EstrellaDbContext dbContext)
    {
        categories = [.. dbContext.Categories.Include(c => c.SubCategories).OrderBy(c => c.Description)];
        subCategories = [];
        providers = [.. dbContext.Providers.OrderBy(p => p.Name)];

        PropertyMapping.Add(nameof(Product.Description), "Descripción");
        PropertyMapping.Add("Category.Description", "Categoría");
        PropertyMapping.Add("SubCategory.Description", "Sub Categoría");
        PropertyMapping.Add("Provider.Name", "Proveedor");
        foreach (string value in PropertyMapping.Values)
        {
            SortOptions.Add(value);
        }
        SelectedOption = SortOptions[0];
        SelectedDirection = SortDirection[0];
    }
    public override bool Filter(object o)
    {
        if (o is not Product product) return false;

        bool ret = product.Code.Contains(ProductCode);
        ret &= product.Description.Contains(ProductDescription);
        ret &= product.Barcode.Contains(ProductBarcode);
        
        if (SelectedCategory != null)
        {
            ret &= product.Category == SelectedCategory;
        }
        if (SelectedSubCategory != null)
        {
            ret &= product.SubCategory == SelectedSubCategory;
        }
        if (SelectedProvider != null)
        {
            ret &= product.Provider == SelectedProvider;
        }

        return ret;
    }

    public override SortDescription GetSortDescription()
    {
        string key = PropertyMapping.First(x => x.Value == SelectedOption).Key;
        ListSortDirection direction = SelectedDirection == SortDirection[0] ? ListSortDirection.Ascending : ListSortDirection.Descending;
        return new(key, direction);
    }

    partial void OnSelectedCategoryChanged(Category? value)
    {
        if (value is null)
        {
            SubCategories = [];
            return;
        }

        SubCategories = new(value.SubCategories.OrderBy(sc => sc.Description));
    }
}
