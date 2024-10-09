using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace EstrellaAccesoriosWpf.Models;

public partial class Sell : ObservableObject
{
    public Guid Id { get; set; }

    [ObservableProperty]
    private ObservableCollection<SellItem> items;

    [ObservableProperty]
    private decimal fixedDiscount;

    [ObservableProperty]
    private decimal percentageDiscount;

    [ObservableProperty]
    private decimal totalPrice;

    [ObservableProperty]
    private decimal subTotalPrice;

    [ObservableProperty]
    private decimal totalEarned;

    [ObservableProperty]
    private PaymentMethod paymentMethod;

    [ObservableProperty]
    private DateTime date = DateTime.Now;

    private Sell(Guid id, IEnumerable<SellItem> items, decimal fixedDiscount, decimal percentageDiscount, PaymentMethod paymentMethod)
    {
        Id = id;
        Items = new(items);

        SubTotalPrice = 0m;
        TotalEarned = 0m;

        foreach (SellItem item in items) subTotalPrice += item.ProductCount * item.ProductPrice;
        decimal tempDiscount = subTotalPrice / 100m * percentageDiscount;

        FixedDiscount = fixedDiscount;
        PercentageDiscount = percentageDiscount;
        SubTotalPrice = subTotalPrice;
        TotalPrice = SubTotalPrice - tempDiscount - fixedDiscount;
        PaymentMethod = paymentMethod;
    }



    public static Sell Create(IEnumerable<SellItem> items, decimal fixedDiscount, decimal percentageDiscount, PaymentMethod paymentMethod)
    {
        return new(Guid.NewGuid(), items, fixedDiscount, percentageDiscount, paymentMethod);
    }

    public void AddSellItem(SellItem item)
    {
        if (Items.Any(i => i.ProductCode == item.ProductCode))
        {
            MessageBox.Show("Ya agregaste este producto.");
            return;
        }

        SubTotalPrice += item.ProductPrice * item.ProductCount;
        item.PropertyChanged += Item_PropertyChanged;
        Items.Add(item);
    }

    private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SellItem.ProductCount))
        {
            SubTotalPrice = Items.Sum(item => item.ProductPrice * item.ProductCount);
        }
    }

    public void RemoveSellItem(SellItem item)
    {
        if (Items.Any(i => i.ProductCode == item.ProductCode))
        {
            SubTotalPrice -= item.ProductPrice * item.ProductCount;
            item.PropertyChanged -= Item_PropertyChanged;
            Items.Remove(item);
        }

    }
    private void CalculateNewTotal(decimal subTotal, decimal percentageDiscount, decimal fixedDiscount, PaymentMethod paymentMethod)
    {
        decimal percentagePaymentAdjustment = paymentMethod.IsDiscount ? -paymentMethod.Amount : paymentMethod.Amount;
        TotalPrice = subTotal * (1 - (percentageDiscount - percentagePaymentAdjustment) / 100) - fixedDiscount;
    }
    partial void OnPaymentMethodChanged(PaymentMethod value)
    {
        CalculateNewTotal(SubTotalPrice, PercentageDiscount, FixedDiscount, value);
    }

    partial void OnSubTotalPriceChanged(decimal value)
    {
        CalculateNewTotal(value, PercentageDiscount, FixedDiscount, PaymentMethod);
    }
    partial void OnFixedDiscountChanged(decimal value)
    {
        CalculateNewTotal(SubTotalPrice, PercentageDiscount, value, PaymentMethod);
    }
    partial void OnPercentageDiscountChanged(decimal value)
    {
        CalculateNewTotal(SubTotalPrice, value, FixedDiscount, PaymentMethod);
    }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    private Sell() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
