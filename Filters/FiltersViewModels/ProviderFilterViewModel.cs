using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class ProviderFilterViewModel : BaseFilter
{
    [ObservableProperty]
    string providerName = string.Empty;
    public ProviderFilterViewModel()
    {
        PropertyMapping.Add(nameof(Provider.Name), "Nombre");
        foreach (string value in PropertyMapping.Values)
        {
            SortOptions.Add(value);
        }
        SelectedOption = SortOptions[0];
        SelectedDirection = SortDirection[0];
    }

    public override bool Filter(object o)
    {
        if (o is not Provider provider) return false;

        return provider.Name.Contains(ProviderName);
    }

    public override SortDescription GetSortDescription()
    {
        string key = PropertyMapping.First(x => x.Value == SelectedOption).Key;
        ListSortDirection direction = SelectedDirection == SortDirection[0] ? ListSortDirection.Ascending : ListSortDirection.Descending;
        return new(key, direction);
    }
    
}
