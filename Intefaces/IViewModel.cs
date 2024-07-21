using CommunityToolkit.Mvvm.Input;

namespace EstrellaAccesoriosWpf.Intefaces;

public interface IViewModel
{
    [RelayCommand]
    Task Load();
    [RelayCommand]
    Task Close();
}
