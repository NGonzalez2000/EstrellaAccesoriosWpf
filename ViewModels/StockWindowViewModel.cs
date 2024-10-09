
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Filters.FiltersViewModels;
using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class StockWindowViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private ICollectionView collectionView = null!;
    private ProductViewModel _parentViewModel = null!;
    private Action closeAction = null!;
    private TaskCompletionSource<StockIncome> _closeTaskCompletionSource = null!;

    [ObservableProperty]
    SellWindowFilterViewModel filterViewModel = null!;

    [ObservableProperty]
    List<PaymentMethod> paymentMethods = null!;

    [ObservableProperty]
    List<Category> categories = null!;

    [ObservableProperty]
    List<SubCategory> subCategories = null!;

    [ObservableProperty]
    ObservableCollection<Product> products = null!;

    [ObservableProperty]
    StockIncome stock = null!;
    public StockWindowViewModel(EstrellaDbContext dbContext)
    {
        _dbContext = dbContext;
        FilterViewModel = new(dbContext);
        FilterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
    }

    public void SetParentViewModel(ProductViewModel parentViewModel)
    {
        _parentViewModel = parentViewModel;
    }
    private void SetFilter()
    {
        collectionView = CollectionViewSource.GetDefaultView(Products);
        collectionView.Filter = FilterViewModel.Filter;
        Refresh();
    }
    public void SetAction(Action action)
    {
        closeAction = action;
    }
    public void SetReturnTask(TaskCompletionSource<StockIncome> task)
    {
        _closeTaskCompletionSource = task;
    }
    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    protected async override Task LoadAsync()
    {
        PaymentMethods = await _dbContext.PaymentMethods.ToListAsync();
        Categories = await _dbContext.Categories.ToListAsync();
        Products = new(await _dbContext.Products.ToListAsync());
        GenerateNewStock();
        SetFilter();
    }

    private void GenerateNewStock()
    {
        Stock = StockIncome.Create();
    }

    protected override void Refresh()
    {
        collectionView.SortDescriptions.Clear();
        collectionView.SortDescriptions.Add(FilterViewModel.GetSortDescription());
        collectionView.Refresh();
    }

    protected async override Task UnloadAsync()
    {
    }

    [RelayCommand]
    private void AddStockItem(Product product)
    {
        var item = Stock.Items.FirstOrDefault(si => si.ProductId == product.Id);
        if (item is null)
        {
            item = StockItem.Create(product.Id, product.Barcode, product.Description, product.ImageSource, 0, product.ListPrice);
            Stock.AddStockItem(item);
        }

        IncreaseCount(item);
    }
    public void AddStockItem(string QrCode)
    {
        var product = Products.FirstOrDefault(p => p.Barcode == QrCode);

        if (product is null)
        {
            MessageBox.Show($"NO SE ENCONTRO EL CODIGO DE BARRA {QrCode}");
            return;
        }

        AddStockItem(product);
    }

    [RelayCommand]
    private void RemoveStockItem(StockItem sellItem)
    {
        Stock.RemoveStockItem(sellItem);
    }

    [RelayCommand]
    private static void IncreaseCount(StockItem item)
    {
        item.ProductCount++;
    }

    [RelayCommand]
    private static void DecreaseCount(StockItem item)
    {
        if (item.ProductCount > 0)
        {
            item.ProductCount--;
        }
    }

    [RelayCommand]
    private void CloseStock()
    {
        if (MessageBox.Show("Esta conforme con la carga de STOCK?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
            return;
        }
        if (Stock.Items.Count == 0)
        {
            MessageBox.Show("NO SE PUEDEN GENERAR UNA CARGA SIN ITEMS.");
            return;
        }

        //_dbContext.Stocks.Add(Stock);
        //await _dbContext.SaveChangesAsync();

        _closeTaskCompletionSource.SetResult(Stock);

        closeAction.Invoke();
        return;
    }
}
