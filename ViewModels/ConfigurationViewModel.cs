using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models.Common;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class ConfigurationViewModel(ISnackbarMessageQueue SnackbarMessageQueue, ConfigurationFile configurations) : ViewModel
{
    [ObservableProperty]
    string wordPath = string.Empty;

    protected override async Task LoadAsync()
    {
        WordPath = configurations.GetWordPath();

    }

    protected override void Refresh()
    {
    }

    protected override async Task UnloadAsync()
    {
        
    }

    [RelayCommand]
    private void SaveStorage()
    {
        configurations.UpdateWordPath(WordPath);
        SnackbarMessageQueue.Enqueue("ALMACENAMIENTOS ACTUALIZADOS");
    }

    
}
