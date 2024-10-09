using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class StockItem : ObservableObject
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    [ObservableProperty]
    private string productCode;

    [ObservableProperty]
    private string productDescription;

    [ObservableProperty]
    private string productImageSource;

    [ObservableProperty]
    private int productCount;

    [ObservableProperty]
    private decimal productListPrice;


    private StockItem(Guid id, Guid productId, string productCode, string productDescription, string productImageSource, int productCount, decimal productListPrice)
    {
        Id = id;
        ProductId = productId;
        ProductCode = productCode;
        ProductDescription = productDescription;
        ProductImageSource = productImageSource;
        ProductCount = productCount;
        ProductListPrice = productListPrice;
    }
    public static StockItem Create(Guid productId, string productCode, string productDescription, string productImageSource, int productCount, decimal productListPrice)
    {
        return new(Guid.NewGuid(), productId, productCode, productDescription, productImageSource, productCount, productListPrice);
    }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    private StockItem() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
