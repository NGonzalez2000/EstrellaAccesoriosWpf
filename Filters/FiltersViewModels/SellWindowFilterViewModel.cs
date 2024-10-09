using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class SellWindowFilterViewModel : BaseFilter
{
    private readonly EstrellaDbContext _dbContext;

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
    public SellWindowFilterViewModel(EstrellaDbContext dbContext)
    {
        _dbContext = dbContext;
        categories = new(GetCategories());

        subCategories = [];
        providers = new(GetProviders());

        SelectedProvider = Providers[0];

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
    private IEnumerable<Provider> GetProviders()
    {
        yield return Provider.Create("TODOS");
        foreach(Provider provider in _dbContext.Providers.OrderBy(p => p.Name))
        {
            yield return provider;
        }
    }

    private IEnumerable<Category> GetCategories()
    {
        yield return Category.Create("TODAS");

        foreach(Category category in _dbContext.Categories.Include(c => c.SubCategories).OrderBy(c => c.Description))
        {
            yield return category;
        }
    }

    private static IEnumerable<SubCategory> GetSubCategories(Category category)
    {
        yield return SubCategory.Create("TODAS");

        foreach (SubCategory subCategory in category.SubCategories.OrderBy(sc => sc.Description))
        {
            yield return subCategory;
        }
    }


    public override bool Filter(object o)
    {
        if (o is not Product product) return false;

        bool ret = product.Code.Contains(ProductCode);
        ret &= product.Description.Contains(ProductDescription);
        ret &= product.Barcode.Contains(ProductBarcode);

        if (SelectedCategory != null && SelectedCategory.Description != "TODAS")
        {
            ret &= product.Category == SelectedCategory;
        }
        if (SelectedSubCategory != null && SelectedSubCategory.Description != "TODAS")
        {
            ret &= product.SubCategory == SelectedSubCategory;
        }
        if(SelectedProvider != null && SelectedProvider.Name != "TODOS")
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

        if(value.Description == "TODAS")
        {
            SubCategories = [];
            return;
        }

        SubCategories = new(GetSubCategories(value));

        SelectedSubCategory = SubCategories[0];
    }
}
