using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Filters.FiltersViewModels;
using EstrellaAccesoriosWpf.Models;
using EstrellaAccesoriosWpf.Popups;
using EstrellaAccesoriosWpf.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class StockIncomeViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private readonly ISnackbarMessageQueue _messageQueue;
    private readonly IConfiguration _configuration;
    private readonly string qrFolder;
    private StockWindow _stockWindow;

    [ObservableProperty]
    private StockIncomeFilterViewModel filterViewModel = null!;

    [ObservableProperty]
    ObservableCollection<StockIncome> stockIncomes = null!;
    private ICollectionView collectionView = null!;

    [ObservableProperty]
    private StockIncome? selectedItem;

    public StockIncomeViewModel(IConfiguration configuration,StockWindow stockWindow, EstrellaDbContext dbContext, ISnackbarMessageQueue messageQueue)
    {
        _stockWindow = stockWindow;
        _dbContext = dbContext;
        _messageQueue = messageQueue;
        _configuration = configuration;
        //string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //qrFolder = Path.Combine(appData, "EstrellaWpf", "Qrs");
    }
    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    protected async override Task LoadAsync()
    {
        StockIncomes = new(await _dbContext.StockIncomes.Include(si => si.Items).ToListAsync());
        FilterViewModel = new();
        FilterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
        SetFilter();
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

    private void SetFilter()
    {
        collectionView = CollectionViewSource.GetDefaultView(StockIncomes);
        collectionView.Filter = FilterViewModel.Filter;
        Refresh();
    }

    [RelayCommand]
    private async Task View()
    {
        if(SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un Ingreso");
            return;
        }

        StockIncomePopup popup = new(SelectedItem);
        await popup.View();
    }

    [RelayCommand]
    private void GenerateTickets()
    {
        if(SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un Ingreso");
            return;
        }


    }
    [RelayCommand]
    private async Task OpenStockWindow()
    {
        TaskCompletionSource<StockIncome> _taskCompletionSource = new();

        _stockWindow.SetReturnTask(_taskCompletionSource);
        _stockWindow.Show();

        try
        {
            var result = await _taskCompletionSource.Task;
            if (_taskCompletionSource.Task.IsCompletedSuccessfully)
            {
                AddItem(result);
            }
        }
        catch (Exception)
        {

        }
    }
    public void AddItem(StockIncome newStock)
    {
        _dbContext.StockIncomes.Add(newStock);
        StockIncomes.Add(newStock);

        List<Product> tempProducts = [.. _dbContext.Products];
        List<Product> updatedProducts = [];
        Product? temp;
        foreach (var item in newStock.Items)
        {
            temp = tempProducts.FirstOrDefault(p => p.Id == item.ProductId);
            if (temp is null) continue;

            temp.Stock += item.ProductCount;
            updatedProducts.Add(temp);
        }


        _dbContext.Products.UpdateRange(updatedProducts);
        _dbContext.SaveChanges();

        _messageQueue.Enqueue("Stock cargado con éxito.");
    }
    
}
