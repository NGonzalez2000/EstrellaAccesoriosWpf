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

public partial class ProductViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private readonly ISnackbarMessageQueue _messageQueue;
    private ICollectionView collectionView = null!;
    private List<Category> categories;
    private List<Provider> providers;


    [ObservableProperty]
    private Product? selectedItem;

    [ObservableProperty]
    private ProductFilterViewModel filterViewModel = null!;

    [ObservableProperty]
    private ObservableCollection<Product> products;



    public ProductViewModel(EstrellaDbContext dbContext, ISnackbarMessageQueue SnackbarMessageQueue)
    {
        _dbContext = dbContext;
        _messageQueue = SnackbarMessageQueue;
        Products = [];
        categories = [];
        providers = [];
        
    }
    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    protected override async Task LoadAsync()
    {
        FilterViewModel = new(_dbContext);
        FilterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
        Products = new(await _dbContext.Products.ToListAsync());
        categories = await _dbContext.Categories.Include(c => c.SubCategories).ToListAsync();
        providers = await _dbContext.Providers.ToListAsync();
        SetFilter();
    }

    protected override async Task UnloadAsync()
    {
        await Task.CompletedTask;
        Products = [];
    }
    private void SetFilter()
    {
        collectionView = CollectionViewSource.GetDefaultView(Products);
        collectionView.Filter = FilterViewModel.Filter;
    }

    [RelayCommand]
    private async Task Create()
    {
        Provider provider = providers.First(p => p.Name == "SIN PROVEEDOR");
        Category category = categories.First(c => c.Description == "SIN CATEGORIA");
        SubCategory subCategory = category.SubCategories.First(sc => sc.Description == "SIN SUBCATEGORIA");

        var maxBarCode = _dbContext.Products
            .Where(p => p.Barcode.StartsWith("EA"))
            .AsEnumerable()
            .Select(p => long.Parse(p.Barcode[2..]))
            .Max();

        Product newItem = Product.Create(category, subCategory, provider);
        newItem.Barcode = $"EA{++maxBarCode:D13}";
        ProductPopup popup = new(newItem, _dbContext);
        bool response = await popup.Create();
        if (!response)
        {
            //ImageManager.DeleteImage(newItem.ImageSource);
            return;
        }


        _dbContext.Products.Add(newItem);
        await _dbContext.SaveChangesAsync();

        Products.Add(newItem);
        SetFilter();

        _messageQueue.Enqueue("Producto creada con éxito");
    }

    [RelayCommand]
    private async Task Update()
    {
        if (SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar una Categoría.");
            return;
        }

        Product editedProduct = Product.Create(SelectedItem.Code,
                                               SelectedItem.Barcode,
                                               SelectedItem.Description,
                                               SelectedItem.ListPrice,
                                               SelectedItem.SalePrice,
                                               SelectedItem.Stock,
                                               SelectedItem.Category,
                                               SelectedItem.SubCategory,
                                               SelectedItem.Provider,
                                               SelectedItem.ImageSource);

        ProductPopup popup = new(editedProduct, _dbContext);
        bool response = await popup.Update();
        if (!response) return;


        SelectedItem.Update(editedProduct);

        if (_dbContext.Entry(SelectedItem).State == EntityState.Detached)
        {
            _dbContext.Attach(SelectedItem);
        }
        _dbContext.Entry(SelectedItem).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        _messageQueue.Enqueue("Producto editado con éxito");
    }
    [RelayCommand]
    private async Task Delete()
    {
        if (SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un Producto.");
            return;
        }

        if (MessageBox.Show($"Seguro que desea eliminar el Producto {SelectedItem.Description}?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
            return;
        }

        _dbContext.Products.Remove(SelectedItem);
        await _dbContext.SaveChangesAsync();

        Products.Remove(SelectedItem);

        _messageQueue.Enqueue("Producto eliminado con éxito.");
    }
    
   
    protected override void Refresh()
    {
        collectionView.SortDescriptions.Clear();
        collectionView.SortDescriptions.Add(FilterViewModel.GetSortDescription());
        collectionView.Refresh();
    }
}
