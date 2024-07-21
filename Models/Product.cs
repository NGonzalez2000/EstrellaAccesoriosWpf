using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Reflection;

namespace EstrellaAccesoriosWpf.Models;

public partial class Product : ObservableObject
{
    public Guid Id;
    [ObservableProperty]
    private string code;

    [ObservableProperty]
    private string barcode;

    [ObservableProperty]
    private string description;

    [ObservableProperty]
    private decimal listPrice;

    [ObservableProperty]
    private decimal salePrice;

    [ObservableProperty]
    private int stock;
   
    [ObservableProperty]
    private Category category;

    [ObservableProperty]
    private SubCategory subCategory;

    [ObservableProperty]
    private Provider provider;

    [ObservableProperty]
    private string imageSource;
    
    private Product(Guid id, string code,string barcode, string description, decimal listPrice, decimal salePrice, int stock,Category category, SubCategory subCategory, Provider provider, string imageSource)
    {
        Id = id;
        Code = code;
        Barcode = barcode;
        Description = description;
        ListPrice = listPrice;
        SalePrice = salePrice;
        Stock = stock;
        Category = category;
        SubCategory = subCategory;
        Provider = provider;
        ImageSource = imageSource;
    }
    public static Product Create(Category category, SubCategory subCategory, Provider provider)
    {
        string assemblyPath = Directory.GetCurrentDirectory();
        string imagePath = Path.Combine(assemblyPath, "Resources\\Images\\default_product.jpeg");
        return new(Guid.NewGuid(),"","","",0m,0m,0, category, subCategory, provider, imagePath);
    }
    public static Product Create(string code, string barcode, string description, decimal listPrice, decimal salePrice, int stock, Category category, SubCategory subCategory, Provider provider, string imageSource)
    {
        return new(Guid.NewGuid(), code, barcode, description, listPrice, salePrice, stock, category, subCategory, provider, imageSource);
    }
    public void Update(Product product)
    {
        Code = product.Code;
        Barcode = product.Barcode;
        Description = product.Description;
        ListPrice = product.ListPrice;
        SalePrice = product.SalePrice;
        Stock = product.Stock;
        Category = product.Category;
        SubCategory = product.SubCategory;
        Provider = product.Provider;
    }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    private Product() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

}
