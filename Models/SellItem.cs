using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class SellItem : ObservableObject
{
    public Guid Id { get; set; }

    [ObservableProperty]
    private string productCode;

    [ObservableProperty]
    private string productDescription;

    [ObservableProperty]
    private string productImageSource;

    [ObservableProperty]
    private int productCount;

    [ObservableProperty]
    private decimal productPrice;

    private SellItem(Guid id, string productCode, string productDescription, string productImageSource, int productCount, decimal productPrice)
    {
        Id = id;
        ProductCode = productCode;
        ProductDescription = productDescription;
        ProductImageSource = productImageSource;
        ProductCount = productCount;
        ProductPrice = productPrice;
    }
    public static SellItem Create(string productCode, string productDescription, string productImageSource, int productCount, decimal productPrice)
    {
        return new(Guid.NewGuid(),  productCode, productDescription, productImageSource, productCount, productPrice);
    }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    private SellItem() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}   

