using EstrellaAccesoriosWpf.Common;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.ViewModels;

public class PaymentMeanViewModel(ISnackbarMessageQueue SnackbarMessageQueue) : ViewModel
{
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
