using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace EstrellaAccesoriosWpf.Models;

public partial class StockIncome : ObservableObject
{

    public Guid Id { get; set; }

    [ObservableProperty]
    private ObservableCollection<StockItem> items;

    [ObservableProperty]
    private decimal totalPrice;

    [ObservableProperty]
    private DateOnly date = DateOnly.FromDateTime(DateTime.Now);

    private StockIncome(Guid id)
    {
        Id = id;
        Items = [];
        TotalPrice = 0m;
    }



    public static StockIncome Create()
    {
        return new(Guid.NewGuid());
    }

    public void AddStockItem(StockItem item)
    {
        if (Items.Any(i => i.ProductCode == item.ProductCode))
        {
            MessageBox.Show("Ya agregaste este producto.");
            return;
        }

        TotalPrice += item.ProductListPrice * item.ProductCount;
        item.PropertyChanged += Item_PropertyChanged;
        Items.Add(item);
    }

    private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(StockItem.ProductCount))
        {
            TotalPrice = Items.Sum(item => item.ProductListPrice * item.ProductCount);
        }
    }

    public void RemoveStockItem(StockItem item)
    {
        if (Items.Any(i => i.ProductCode == item.ProductCode))
        {
            TotalPrice -= item.ProductListPrice * item.ProductCount;
            item.PropertyChanged -= Item_PropertyChanged;
            Items.Remove(item);
        }

    }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    private StockIncome() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
