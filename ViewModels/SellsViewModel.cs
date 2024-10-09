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

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class SellsViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private readonly ISnackbarMessageQueue _messageQueue;
    private readonly SellWindow _sellWindow;
    private ICollectionView collectionView = null!;

    [ObservableProperty]
    Sell? selectedItem;

    [ObservableProperty]
    ObservableCollection<Sell> sells = null!;

    [ObservableProperty]
    SellFilterViewModel filterViewModel = null!;

    public SellsViewModel(SellWindow sellWindow, EstrellaDbContext dbContext, ISnackbarMessageQueue SnackbarMessageQueue)
    {
        Sells = [];
        _sellWindow = sellWindow;
        
        if(_sellWindow.DataContext is SellWindowViewModel vm)
        {
            vm.SetParentViewModel(this);
        }

        _dbContext = dbContext;
        _messageQueue = SnackbarMessageQueue;

        
        filterViewModel = new(dbContext);
    }
    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    protected override async Task LoadAsync()
    {
        FilterViewModel = new(_dbContext);
        FilterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
        Sells = new(await _dbContext.Sells.ToListAsync());
        SetFilter();
    }
    
    private void SetFilter()
    {
        collectionView = CollectionViewSource.GetDefaultView(Sells);
        collectionView.Filter = FilterViewModel.Filter;
        Refresh();
    }

    protected override void Refresh()
    {
        collectionView.SortDescriptions.Clear();
        collectionView.SortDescriptions.Add(FilterViewModel.GetSortDescription());
        collectionView.Refresh();
    }

    protected override Task UnloadAsync()
    {
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task Create()
    {
        TaskCompletionSource<Sell> _taskCompletionSource = new();

        _sellWindow.SetReturnTask(_taskCompletionSource);
        _sellWindow.Show();

        try
        {
            var result = await _taskCompletionSource.Task;
            if(_taskCompletionSource.Task.IsCompletedSuccessfully)
            {
                await AddItemAsync(result);
            }
        }
        catch (Exception)
        {

        }

    }

    [RelayCommand]
    private async Task View()
    {
        if(SelectedItem is null)
        {
            MessageBox.Show("DEBE SELECCIONAR UNA VENTA");
            return;
        }
        if(SelectedItem.Items is null)
        {
            SelectedItem = _dbContext.Sells.Include(s => s.Items).First(s => s.Id == SelectedItem.Id);
        }
        SellPopup popup = new(SelectedItem);
        await popup.View();
    }
    public async Task AddItemAsync(Sell newSell)
    {
        _dbContext.Sells.Add(newSell);

        Sells.Add(newSell);

        MoneyMovementType sellType = _dbContext.MoneyMovementTypes.First(x => x.Description == "VENTA");

        MoneyMovement moneyMovement = MoneyMovement.Create("VENTA", newSell.TotalEarned, sellType, newSell.Id);
        moneyMovement.PaymentMethod = newSell.PaymentMethod;
        _dbContext.MoneyMovements.Add(moneyMovement);

        await _dbContext.SaveChangesAsync();

        _messageQueue.Enqueue("Venta generada con éxito.");
    }
}
