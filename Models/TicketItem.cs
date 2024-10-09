using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class TicketItem : ObservableObject
{

    [ObservableProperty]
    string code;

    [ObservableProperty]
    string barcode;

    [ObservableProperty]
    string description;

    [ObservableProperty]
    int quantity;

    public TicketItem()
    {
        code = string.Empty;
        barcode = string.Empty;
        description = string.Empty;
        quantity = 0;
    }
}
