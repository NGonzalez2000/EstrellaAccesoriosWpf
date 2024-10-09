using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Filters.FiltersViewModels;
using EstrellaAccesoriosWpf.Models;
using EstrellaAccesoriosWpf.Popups;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class PaymentMethodViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private readonly ISnackbarMessageQueue _messageQueue;
    private ICollectionView collectionView = null!;

    [ObservableProperty]
    private ObservableCollection<PaymentMethod> paymentMethods;

    [ObservableProperty]
    private PaymentMethodFilterViewModel filterViewModel;

    [ObservableProperty]
    private PaymentMethod? selectedItem;
    public PaymentMethodViewModel(EstrellaDbContext dbContext, ISnackbarMessageQueue SnackbarMessageQueue)
    {
        _dbContext = dbContext;
        _messageQueue = SnackbarMessageQueue;

        PaymentMethods = [];
        filterViewModel = new();
        filterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
    }
    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    protected override async Task LoadAsync()
    {
        PaymentMethods = new(await _dbContext.PaymentMethods.ToListAsync());
        collectionView = CollectionViewSource.GetDefaultView(PaymentMethods);
        collectionView.Filter = FilterViewModel.Filter;
    }

    protected override void Refresh()
    {
        collectionView.SortDescriptions.Clear();
        collectionView.SortDescriptions.Add(FilterViewModel.GetSortDescription());
        collectionView.Refresh();
    }
    protected override async Task UnloadAsync()
    {
        await Task.CompletedTask;
        PaymentMethods.Clear();
    }

    [RelayCommand]
    private async Task Create()
    {
        PaymentMethod newItem = PaymentMethod.Create("", 0m, true);
        PaymentMethodPopup popup = new(newItem);
        bool response = await popup.Create();
        if (!response) return;


        _dbContext.PaymentMethods.Add(newItem);
        await _dbContext.SaveChangesAsync();

        PaymentMethods.Add(newItem);

        _messageQueue.Enqueue("Medio de pago creado con éxito");
    }
    [RelayCommand]
    private async Task Update()
    {
        if (SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un Medio de pago.");
            return;
        }

        PaymentMethod editedPaymentMethod = PaymentMethod.Create(SelectedItem.Description, SelectedItem.Amount, SelectedItem.IsDiscount);

        PaymentMethodPopup popup = new(editedPaymentMethod);
        bool response = await popup.Update();
        if (!response) return;


        SelectedItem.Update(editedPaymentMethod);

        if (_dbContext.Entry(SelectedItem).State == EntityState.Detached)
        {
            _dbContext.Attach(SelectedItem);
        }
        _dbContext.Entry(SelectedItem).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        _messageQueue.Enqueue("Medio de pago editado con éxito");
    }
    [RelayCommand]
    private async Task Delete()
    {
        if (SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un Medio de pago.");
            return;
        }

        if (MessageBox.Show($"Seguro que desea eliminar el Medio de pago {SelectedItem.Description}?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
            return;
        }

        _dbContext.PaymentMethods.Remove(SelectedItem);
        await _dbContext.SaveChangesAsync();

        PaymentMethods.Remove(SelectedItem);

        _messageQueue.Enqueue("Medio de pago eliminado con éxito.");
    }
}
