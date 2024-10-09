using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class PaymentMethod : ObservableObject
{
    public Guid Id { get; set; }
    [ObservableProperty]
    private string description;

    [ObservableProperty]
    private decimal amount;

    [ObservableProperty]
    private bool isDiscount;

    [ObservableProperty]
    private bool modifyCash;
    private PaymentMethod(Guid id, string description, decimal amount, bool isDiscount)
    {
        Id = id;
        Description = description;
        Amount = amount;
        IsDiscount = isDiscount;
    }
    public static PaymentMethod Create(string description, decimal amount, bool isDiscount)
    {
        return new(Guid.NewGuid(), description, amount, isDiscount);
    }

    public void Update(PaymentMethod paymentMethod)
    {
        Description = paymentMethod.Description;
        Amount = paymentMethod.Amount;
        IsDiscount = paymentMethod.IsDiscount;
        ModifyCash = paymentMethod.ModifyCash;
    }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public PaymentMethod() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
