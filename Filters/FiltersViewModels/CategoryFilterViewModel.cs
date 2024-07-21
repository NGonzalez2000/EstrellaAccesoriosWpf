using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class CategoryFilterViewModel : BaseFilter
{ 
    [ObservableProperty]
    string categoryDescription = string.Empty;
    public CategoryFilterViewModel()
    {
        PropertyMapping.Add(nameof(Category.Description), "Descripción");
        foreach(string value in PropertyMapping.Values)
        {
            SortOptions.Add(value);
        }
        SelectedOption = SortOptions[0];
        SelectedDirection = SortDirection[0];
    }

    public override bool Filter(object o)
    {
        if (o is not Category cat) return false;

        return cat.Description.Contains(CategoryDescription);
    }

    public override SortDescription GetSortDescription()
    {
        string key = PropertyMapping.First(x => x.Value == SelectedOption).Key;
        ListSortDirection direction = SelectedDirection == SortDirection[0] ? ListSortDirection.Ascending : ListSortDirection.Descending;
        return new(key, direction);
    }
}
