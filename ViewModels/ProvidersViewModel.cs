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

public partial class ProvidersViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private readonly ISnackbarMessageQueue _messageQueue;
    private ICollectionView collectionView = null!;

    [ObservableProperty]
    private Provider? selectedItem;
    
    [ObservableProperty]
    private ProviderFilterViewModel filterViewModel;
    
    [ObservableProperty]
    private ObservableCollection<Provider> providers; 
    
    public ProvidersViewModel(EstrellaDbContext dbContext, ISnackbarMessageQueue SnackbarMessageQueue)
    {
        _dbContext = dbContext;
        _messageQueue = SnackbarMessageQueue;
        Providers = [];
        filterViewModel = new();
        filterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
    }
    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    protected override async Task LoadAsync()
    {
        Providers = new(await _dbContext.Providers.Where(x => x.Name != "SIN PROVEEDOR").ToListAsync());
        collectionView = CollectionViewSource.GetDefaultView(Providers);
        collectionView.Filter = FilterViewModel.Filter;
    }

    protected override async Task UnloadAsync()
    {
        Providers = [];
        await Task.CompletedTask;
    }
    [RelayCommand]
    private async Task Create()
    {
        Provider newProvider = Provider.Create("");
        ProviderPopup popup = new(newProvider);
        bool response = await popup.Create();
        if (!response) return;

        
        _dbContext.Providers.Add(newProvider);
        await _dbContext.SaveChangesAsync();

        Providers.Add(newProvider);

        _messageQueue.Enqueue("Proveedor creado con éxito");
    }
    [RelayCommand]
    private async Task Update()
    {
        if(SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un Proveedor.");
            return;
        }

        Provider editedProvider = Provider.Create(SelectedItem.Name);

        ProviderPopup popup = new(editedProvider);
        bool response = await popup.Update();
        if (!response) return;


        SelectedItem.Update(editedProvider);

        if(_dbContext.Entry(SelectedItem).State == EntityState.Detached)
        {
            _dbContext.Attach(SelectedItem);
        }
        _dbContext.Entry(SelectedItem).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        _messageQueue.Enqueue("Proveedor editado con éxito");
    }
    [RelayCommand]
    private async Task Delete()
    {
        if(SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un Proveedor.");
            return;
        }

        if(MessageBox.Show($"Seguro que desea eliminar el proveedor {SelectedItem.Name}?","",MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
            return;
        }

        Provider withoutProvider = _dbContext.Providers.First(p => p.Name == "SIN PROVEEDOR");
        foreach(Product product in _dbContext.Products.Where(p => p.Provider.Id == SelectedItem.Id))
        {
            product.Provider = withoutProvider;
            _dbContext.Products.Update(product);
        }

        _dbContext.Providers.Remove(SelectedItem);

        await _dbContext.SaveChangesAsync();

        Providers.Remove(SelectedItem);

        _messageQueue.Enqueue("Proveedor eliminado con éxito.");
    }

    protected override void Refresh()
    {
        collectionView.SortDescriptions.Clear();
        collectionView.SortDescriptions.Add(FilterViewModel.GetSortDescription());
        collectionView.Refresh();
    }
}
