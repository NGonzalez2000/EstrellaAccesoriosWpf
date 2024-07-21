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
using System.Windows.Data;
using System.Windows;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class SubCategoryViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private readonly ISnackbarMessageQueue _messageQueue;
    private ICollectionView collectionView = null!;

    [ObservableProperty]
    private SubCategory? selectedItem;

    [ObservableProperty]
    private Category? selectedCategory;

    [ObservableProperty]
    private SubCategoryFilterViewModel filterViewModel;

    [ObservableProperty]
    private ObservableCollection<Category> categories;

    [ObservableProperty]
    private ObservableCollection<SubCategory> subCategories;

    public SubCategoryViewModel(EstrellaDbContext dbContext, ISnackbarMessageQueue SnackbarMessageQueue)
    {
        _dbContext = dbContext;
        _messageQueue = SnackbarMessageQueue;
        Categories = [];
        subCategories = [];
        filterViewModel = new();
        filterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
    }
    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }
    partial void OnSelectedCategoryChanged(Category? value)
    {
        if (value is null)
        {
            SubCategories = [];
            return;
        }
        SubCategories = new(value.SubCategories.Where(sc => sc.Description != "SIN SUBCATEGORIA"));
        collectionView = CollectionViewSource.GetDefaultView(SubCategories);
        collectionView.Filter = FilterViewModel.Filter;
    }
    protected override async Task LoadAsync()
    {
        Categories = new(await _dbContext.Categories.Where(x => x.Description != "SIN CATEGORIA").Include(c => c.SubCategories).ToListAsync());
        if(Categories.Any())
        {
            SelectedCategory = Categories[0];
        }
    }

    protected override async Task UnloadAsync()
    {
        Categories = [];
        await Task.CompletedTask;
    }
    [RelayCommand]
    private async Task Create()
    {
        if(SelectedCategory is null)
        {
            MessageBox.Show("Debe seleccionar una Categoría.");
            return;
        }
        SubCategory newItem = SubCategory.Create("");
        SubCategoryPopup popup = new(newItem);
        bool response = await popup.Create();
        if (!response) return;

        SelectedCategory.AddSubCategory(newItem);
        _dbContext.Categories.Update(SelectedCategory);
        await _dbContext.SaveChangesAsync();

        SubCategories.Add(newItem);

        _messageQueue.Enqueue("SubCategoría creada con éxito");
    }
    [RelayCommand]
    private async Task Update()
    {
        if(SelectedCategory is null)
        {
            MessageBox.Show("Debe seleccionar una Categoría.");
            return;
        }
        if (SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar una SubCategoría.");
            return;
        }

        SubCategory editedSubCategory = SubCategory.Create(SelectedItem.Description);

        SubCategoryPopup popup = new(editedSubCategory);
        bool response = await popup.Update();
        if (!response) return;

        
        SelectedItem.Update(editedSubCategory);

        if (_dbContext.Entry(SelectedItem).State == EntityState.Detached)
        {
            _dbContext.Attach(SelectedItem);
        }
        _dbContext.Entry(SelectedItem).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        _messageQueue.Enqueue("SubCategoría editado con éxito");
    }
    [RelayCommand]
    private async Task Delete()
    {

        if(SelectedCategory is null)
        {
            MessageBox.Show("Debe seleccionar una Categoría.");
            return;
        }

        if (SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar una SubCategoría.");
            return;
        }

        if (MessageBox.Show($"Seguro que desea eliminar la SubCategoría {SelectedItem.Description}?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
            return;
        }

        SubCategory withoutSubCategory = SelectedCategory.SubCategories.First(p => p.Description == "SIN SUBCATEGORIA");
        foreach (Product product in _dbContext.Products.Where( p => p.Category.Id == SelectedCategory.Id && p.SubCategory.Id == SelectedItem.Id))
        {
            product.SubCategory = withoutSubCategory;
            _dbContext.Products.Update(product);
        }

        SelectedCategory.DeleteSubCategory(SelectedItem);

        _dbContext.Categories.Update(SelectedCategory);
        await _dbContext.SaveChangesAsync();

        SubCategories.Remove(SelectedItem);

        _messageQueue.Enqueue("SubCategoría eliminada con éxito.");
    }

    protected override void Refresh()
    {
        collectionView.SortDescriptions.Clear();
        collectionView.SortDescriptions.Add(FilterViewModel.GetSortDescription());
        collectionView.Refresh();
    }
}
