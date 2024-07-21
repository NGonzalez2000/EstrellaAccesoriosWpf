using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class SubCategoryFilterViewModel : BaseFilter
{
    [ObservableProperty]
    string subCategoryDescription = string.Empty;
    public SubCategoryFilterViewModel()
    {
        PropertyMapping.Add(nameof(SubCategory.Description), "Descripción");
        foreach (string value in PropertyMapping.Values)
        {
            SortOptions.Add(value);
        }
        SelectedOption = SortOptions[0];
        SelectedDirection = SortDirection[0];
    }

    public override bool Filter(object o)
    {
        if (o is not SubCategory subcat) return false;

        return subcat.Description.Contains(SubCategoryDescription);
    }

    public override SortDescription GetSortDescription()
    {
        string key = PropertyMapping.First(x => x.Value == SelectedOption).Key;
        ListSortDirection direction = SelectedDirection == SortDirection[0] ? ListSortDirection.Ascending : ListSortDirection.Descending;
        return new(key, direction);
    }
}
