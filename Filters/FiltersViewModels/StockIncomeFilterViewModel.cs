using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class StockIncomeFilterViewModel : BaseFilter
{
    [ObservableProperty]
    bool rangeDateSearch = false;

    [ObservableProperty]
    DateOnly from;

    [ObservableProperty]
    DateOnly to;

    public StockIncomeFilterViewModel()
    {
        From = To = DateOnly.FromDateTime(DateTime.Now);

        PropertyMapping.Add(nameof(StockIncome.Date), "Fecha");
        foreach (string value in PropertyMapping.Values)
        {
            SortOptions.Add(value);
        }
        SelectedOption = SortOptions[0];
        SelectedDirection = SortDirection[0];
    }
    public override bool Filter(object o)
    {
        if(o is not StockIncome stockIncome)
        {
            return false;
        }
        if(!RangeDateSearch)
        {
            return true;
        }
        return stockIncome.Date >= From && stockIncome.Date <= To;
    }

    public override SortDescription GetSortDescription()
    {
        string key = PropertyMapping.First(x => x.Value == SelectedOption).Key;
        ListSortDirection direction = SelectedDirection == SortDirection[0] ? ListSortDirection.Ascending : ListSortDirection.Descending;
        return new(key, direction);
    }
}
