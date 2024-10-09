using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class PaymentMethodFilterViewModel : BaseFilter
{
    [ObservableProperty]
    private string paymentMethodDescription = string.Empty;

    public PaymentMethodFilterViewModel()
    {
        PropertyMapping.Add(nameof(PaymentMethod.Description), "Descripción");
        foreach (string value in PropertyMapping.Values)
        {
            SortOptions.Add(value);
        }
        SelectedOption = SortOptions[0];
        SelectedDirection = SortDirection[0];
    }
    public override bool Filter(object o)
    {
        if(o is not PaymentMethod paymentMethod)
        {
            return false;
        }
        return paymentMethod.Description.Contains(PaymentMethodDescription);
    }

    public override SortDescription GetSortDescription()
    {
        string key = PropertyMapping.First(x => x.Value == SelectedOption).Key;
        ListSortDirection direction = SelectedDirection == SortDirection[0] ? ListSortDirection.Ascending : ListSortDirection.Descending;
        return new(key, direction);
    }
}
