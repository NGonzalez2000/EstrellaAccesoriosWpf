using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Filters.FiltersViewModels;
using EstrellaAccesoriosWpf.Models;
using EstrellaAccesoriosWpf.Popups;
using EstrellaAccesoriosWpf.Services;
using EstrellaAccesoriosWpf.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class TicketViewModel : ViewModel
{
    private ICollectionView productsCollectionView = null!;
    private ICollectionView stockIncomeCollectionView = null!;
    private readonly EstrellaDbContext _dbContext;
    private readonly string _qrFolderPath;
    private readonly ISnackbarMessageQueue _messageQueue;

    [ObservableProperty]
    Visibility isIncomeVisible;

    [ObservableProperty]
    Visibility isProductsVisible;

    [ObservableProperty]
    bool isIncomeSelected;

    [ObservableProperty]
    ProductFilterViewModel productFilterViewModel = null!;

    [ObservableProperty]
    ObservableCollection<Product> products = null!;

    [ObservableProperty]
    List<Product> selectedProducts;

    [ObservableProperty]
    StockIncomeFilterViewModel stockIncomeFilterViewModel = null!;

    [ObservableProperty]
    ObservableCollection<StockIncome> stockIncomes = null!;

    [ObservableProperty]
    List<StockIncome> selectedStockIncomes;

    [ObservableProperty]
    ObservableCollection<TicketItem> ticketItems;

    [ObservableProperty]
    bool onlyQr;

    [ObservableProperty]
    bool withDescription;

    [ObservableProperty]
    bool isCard;
    public TicketViewModel(IConfiguration configuration, EstrellaDbContext dbContext, ISnackbarMessageQueue messageQueue)
    {
        _qrFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EstrellaWpf", "Qrs");
        _messageQueue = messageQueue;
        _dbContext = dbContext;
        SelectedStockIncomes = [];
        SelectedProducts = [];
        TicketItems = [];
        WithDescription = true;
    }
    partial void OnIsIncomeSelectedChanged(bool value)
    {
        IsIncomeVisible = value ? Visibility.Visible : Visibility.Collapsed;
        IsProductsVisible = !value ? Visibility.Visible : Visibility.Collapsed;
    }

    protected async override Task LoadAsync()
    {
        StockIncomes = new(await _dbContext.StockIncomes.Include(si => si.Items).ToListAsync());
        StockIncomeFilterViewModel = new();
        StockIncomeFilterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;

        Products = new(await _dbContext.Products.ToListAsync());
        ProductFilterViewModel = new(_dbContext);
        ProductFilterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
        SetFilters();

        IsIncomeSelected = true;
    }

    private void SetFilters()
    {
        stockIncomeCollectionView = CollectionViewSource.GetDefaultView(StockIncomes);
        stockIncomeCollectionView.Filter = StockIncomeFilterViewModel.Filter;

        productsCollectionView = CollectionViewSource.GetDefaultView(Products);
        productsCollectionView.Filter = ProductFilterViewModel.Filter;
    }

    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    protected override void Refresh()
    {
        if (IsIncomeSelected)
        {
            stockIncomeCollectionView.SortDescriptions.Clear();
            stockIncomeCollectionView.SortDescriptions.Add(StockIncomeFilterViewModel.GetSortDescription());
            stockIncomeCollectionView.Refresh();
        }
        else
        {
            productsCollectionView.SortDescriptions.Clear();
            productsCollectionView.SortDescriptions.Add(ProductFilterViewModel.GetSortDescription());
            productsCollectionView.Refresh();
        }
    }

    protected async override Task UnloadAsync()
    {
        await Task.CompletedTask;
    }

    [RelayCommand]
    void AddSelectedItems()
    {
        if (IsIncomeSelected)
        {
            foreach (var item in SelectedStockIncomes)
            {
                foreach (var stockItem in item.Items)
                {
                    AddTicketItem(stockItem.ProductId);
                }
            }
        }
        else
        {
            foreach(var product in SelectedProducts)
            {
                AddTicketItem(product.Id);
            }
        }
    }
    [RelayCommand]
    static void Increase(TicketItem? item)
    {
        if(item is not null)
            item.Quantity++;
    }
    [RelayCommand]
    static void Decrease(TicketItem? item)
    {
        if(item is not null && item.Quantity > 0)
            item.Quantity--;
    }

    [RelayCommand]
    async Task GenerateTickets()
    {
        WorkInProgressPopup popup = new("GENERANDO ETIQUETAS");
        Task task = DialogHost.Show(popup, "RootDialog");
        try
        {
            WordService wordService = new(_qrFolderPath, @"C:\Users\Nicolas\Desktop");

            // Run the word service task asynchronously
            await Task.Run(() =>
            {
                if (OnlyQr)
                    wordService.GenerateQrOnlyTickets([.. TicketItems]);
                else if (WithDescription)
                    wordService.GenerateTicketsWithDescription([.. TicketItems]);
                else
                    wordService.GenerateTicketsInCardFormat([.. TicketItems]);
            });
        }
        finally
        {
            // Close the DialogHost after the task is complete
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }

    private void AddTicketItem(Guid productId)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

        if (product is null) return;

        var temp = TicketItems.FirstOrDefault(i => i.Barcode == product.Barcode);

        if (temp is null)
        {
            temp = new()
            {
                Code = product.Code,
                Barcode = product.Barcode,
                Description = product.Description
            };
            TicketItems.Add(temp);
        }

        Increase(TicketItems.First(t => t.Barcode == temp.Barcode));
    }
}
