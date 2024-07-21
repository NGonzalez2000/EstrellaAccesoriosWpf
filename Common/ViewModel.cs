using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EstrellaAccesoriosWpf.Common;

public abstract partial class ViewModel() : ObservableObject
{
    protected abstract Task LoadAsync();
    protected abstract Task UnloadAsync();
    [RelayCommand]
    public async Task Load()
    {
        await LoadAsync();
    }
    [RelayCommand]
    public async Task Unload()
    {
        await UnloadAsync();
    }
    protected abstract void Refresh();
}
