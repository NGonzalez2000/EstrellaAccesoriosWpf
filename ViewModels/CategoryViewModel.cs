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

public partial class CategoryViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private readonly ISnackbarMessageQueue _messageQueue;
    private ICollectionView collectionView = null!;

    [ObservableProperty]
    private Category? selectedItem;

    [ObservableProperty]
    private CategoryFilterViewModel filterViewModel;

    [ObservableProperty]
    private ObservableCollection<Category> categories;

    public CategoryViewModel(EstrellaDbContext dbContext, ISnackbarMessageQueue SnackbarMessageQueue)
    {
        _dbContext = dbContext;
        _messageQueue = SnackbarMessageQueue;
        Categories = [];
        filterViewModel = new();
        filterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
    }
    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    protected override async Task LoadAsync()
    {
        Categories = new(await _dbContext.Categories.Where(x => x.Description != "SIN CATEGORIA").ToListAsync());
        collectionView = CollectionViewSource.GetDefaultView(Categories);
        collectionView.Filter = FilterViewModel.Filter;
    }

    protected override async Task UnloadAsync()
    {
        Categories = [];
        await Task.CompletedTask;
    }
    [RelayCommand]
    private async Task Create()
    {
        Category newItem = Category.Create("");
        CategoryPopup popup = new(newItem);
        bool response = await popup.Create();
        if (!response) return;


        _dbContext.Categories.Add(newItem);
        await _dbContext.SaveChangesAsync();

        Categories.Add(newItem);

        _messageQueue.Enqueue("Categoría creada con éxito");
    }
    [RelayCommand]
    private async Task Update()
    {
        if (SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar una Categoría.");
            return;
        }

        Category editedCategory = Category.Create(SelectedItem.Description);

        CategoryPopup popup = new(editedCategory);
        bool response = await popup.Update();
        if (!response) return;


        SelectedItem.Update(editedCategory);

        if (_dbContext.Entry(SelectedItem).State == EntityState.Detached)
        {
            _dbContext.Attach(SelectedItem);
        }
        _dbContext.Entry(SelectedItem).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        _messageQueue.Enqueue("Categoría editada con éxito");
    }
    [RelayCommand]
    private async Task Delete()
    {
        if (SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar una Categoría.");
            return;
        }

        if (MessageBox.Show($"Seguro que desea eliminar la Categoría {SelectedItem.Description}?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
            return;
        }

        Category withoutCategory = _dbContext.Categories.Include(c => c.SubCategories).First(c => c.Description == "SIN CATEGORIA");
        SubCategory withoutSubCategory = withoutCategory.SubCategories[0];

        foreach(Product product in _dbContext.Products.Where(p => p.Category.Id == SelectedItem.Id))
        {
            product.Category = withoutCategory;
            product.SubCategory = withoutSubCategory;
            _dbContext.Products.Update(product);
        }

        foreach(SubCategory subCategory in SelectedItem.SubCategories)
        {
            _dbContext.SubCategories.Remove(subCategory);
        }

        _dbContext.Categories.Remove(SelectedItem);
        await _dbContext.SaveChangesAsync();

        Categories.Remove(SelectedItem);

        _messageQueue.Enqueue("Categoría eliminada con éxito.");
    }

    protected override void Refresh()
    {
        collectionView.SortDescriptions.Clear();
        collectionView.SortDescriptions.Add(FilterViewModel.GetSortDescription());
        collectionView.Refresh();
    }
}
