using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class MoneyMovementFilterViewModel : BaseFilter
{
    [ObservableProperty]
    string moneyMovementDescription = string.Empty;

    public MoneyMovementFilterViewModel()
    {
        PropertyMapping.Add(nameof(MoneyMovement.Description), "Descripción");
        PropertyMapping.Add("MoneyMovementType.Description", "Operación");
        foreach (string value in PropertyMapping.Values)
        {
            SortOptions.Add(value);
        }
        SelectedOption = SortOptions[0];
        SelectedDirection = SortDirection[0];
    }

    public override bool Filter(object o)
    {
        if (o is not MoneyMovement moneyMovement) return false;

        bool ret = moneyMovement.Description.Contains(MoneyMovementDescription);

        return ret;
    }

    public override SortDescription GetSortDescription()
    {
        string key = PropertyMapping.First(x => x.Value == SelectedOption).Key;
        ListSortDirection direction = SelectedDirection == SortDirection[0] ? ListSortDirection.Ascending : ListSortDirection.Descending;
        return new(key, direction);
    }
}
