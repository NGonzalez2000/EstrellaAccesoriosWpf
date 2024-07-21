using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class PaymentMean : ObservableObject
{
    public Guid Id { get; set; }
    [ObservableProperty]
    private string description;
    private PaymentMean(Guid id, string description)
    {
        Id = id;
        Description = description;
    }
    public static PaymentMean Create(string description)
    {
        return new(Guid.NewGuid(), description);
    }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public PaymentMean() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
