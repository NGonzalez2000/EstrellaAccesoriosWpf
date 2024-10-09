using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Filters.FiltersViewModels;
using EstrellaAccesoriosWpf.Models;
using EstrellaAccesoriosWpf.Popups;
using iText.Commons.Actions.Contexts;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class SellWindowViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private ICollectionView collectionView = null!;
    private SellsViewModel _parentViewModel = null!;
    private Action closeAction = null!;
    private TaskCompletionSource<Sell> _closeTaskCompletionSource = null!;

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
    Sell sell = null!;

    public SellWindowViewModel(EstrellaDbContext dbContext)
    {
        _dbContext = dbContext;
        
    }
    public void SetParentViewModel(SellsViewModel parentViewModel)
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
    public void SetReturnTask(TaskCompletionSource<Sell> task)
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
        FilterViewModel = new(_dbContext);
        FilterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
        GenerateNewSell();
        SetFilter();
    }

    private void GenerateNewSell()
    {
        Sell = Sell.Create([], 0m, 0m, PaymentMethods[0]);
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
    private void AddSellItem(Product product)
    {
        var item = Sell.Items.FirstOrDefault(si => si.ProductCode == product.Barcode);
        if(item is null)
        {
            item = SellItem.Create(product.Barcode, product.Description, product.ImageSource, 0, product.SalePrice);
            Sell.AddSellItem(item);
        }

        IncreaseCount(item);
    }
    public void AddSellItem(string QrCode)
    {
        var product = Products.FirstOrDefault(p => p.Barcode == QrCode);

        if(product is null)
        {
            MessageBox.Show($"NO SE ENCONTRO EL CODIGO DE BARRA {QrCode}");
            return;
        }

        AddSellItem(product);
    }

    [RelayCommand]
    private void RemoveSellItem(SellItem sellItem)
    {
        Sell.RemoveSellItem(sellItem);
    }

    [RelayCommand]
    private static void IncreaseCount(SellItem item)
    {
        item.ProductCount++;
    }

    [RelayCommand]
    private static void DecreaseCount(SellItem item)
    {
        if(item.ProductCount > 0)
        {
            item.ProductCount--;
        }
    }

    [RelayCommand]
    private async Task CloseSell()
    {
        if(MessageBox.Show("Segura que desea cerrar la venta?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
            return;
        }
        if(Sell.Items.Count == 0)
        {
            MessageBox.Show("NO SE PUEDEN GENERAR VENTAS SIN ITEMS.");
            return;
        }

        TotalPopup popup = new(Sell);
        var response = await popup.Show();

        if (!response) return;
        //_dbContext.Sells.Add(Sell);
        //await _dbContext.SaveChangesAsync();

        _closeTaskCompletionSource.SetResult(Sell);

        closeAction.Invoke();
        return;
    }
}
