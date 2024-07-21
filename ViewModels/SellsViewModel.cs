using CommunityToolkit.Mvvm.ComponentModel;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class SellsViewModel : ViewModel
{
    [ObservableProperty]
    List<Product> products;
    public SellsViewModel(EstrellaDbContext _dbContext, ISnackbarMessageQueue SnackbarMessageQueue)
    {
        Products = [];
    }
    protected override Task LoadAsync()
    {
        return Task.CompletedTask;
    }

    protected override void Refresh()
    {
    }

    protected override Task UnloadAsync()
    {
        return Task.CompletedTask;
    }
}
