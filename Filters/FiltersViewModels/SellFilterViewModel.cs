using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Filters.FiltersViewModels;

public partial class SellFilterViewModel : BaseFilter
{
    [ObservableProperty]
    List<PaymentMethod> paymentMethods;

    [ObservableProperty]
    PaymentMethod? selectedPaymentMethod;

    [ObservableProperty]
    bool rangeDateSearch = false;

    [ObservableProperty]
    DateOnly from;

    [ObservableProperty]
    DateOnly to;

    public SellFilterViewModel(EstrellaDbContext dbContext)
    {
        PaymentMethods = new([.. dbContext.PaymentMethods]);
        From = DateOnly.FromDateTime(DateTime.Now);
        To = DateOnly.FromDateTime(DateTime.Now);

        PropertyMapping.Add(nameof(Sell.Date), "Fecha");
        PropertyMapping.Add("PaymentMethod.Description", "Medio de Pago");

        foreach (string value in PropertyMapping.Values)
        {
            SortOptions.Add(value);
        }
        SelectedOption = SortOptions[0];
        SelectedDirection = SortDirection[1];
    }
    public override bool Filter(object o)
    {
        if (o is not Sell sell) return false;

        bool ret = true;

        if (SelectedPaymentMethod != null) 
        {
            ret &= sell.PaymentMethod.Id == SelectedPaymentMethod.Id;
        }

        if (RangeDateSearch)
        {
            ret &= DateOnly.FromDateTime(sell.Date) >= From && DateOnly.FromDateTime(sell.Date) <= To;
        }

        return ret;
    }

    public override SortDescription GetSortDescription()
    {
        string key = PropertyMapping.First(x => x.Value == SelectedOption).Key;
        ListSortDirection direction = SelectedDirection == SortDirection[0] ? ListSortDirection.Ascending : ListSortDirection.Descending;
        return new(key, direction);
    }
}
