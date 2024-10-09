using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;

namespace EstrellaAccesoriosWpf.Models;

public partial class MoneyMovement : ObservableObject
{
    public Guid Id { get; set; }
    public Guid SellGuid { get; set; }
    
    [ObservableProperty]
    private string description;
    
    [ObservableProperty]
    private decimal amount;

    [ObservableProperty]
    private DateOnly date;

    [ObservableProperty]
    private MoneyMovementType moneyMovementType;

    [ObservableProperty]
    private PaymentMethod? paymentMethod;

    private MoneyMovement(Guid id, string description, decimal amount, DateOnly date, Guid sellGuid, MoneyMovementType moneyMovementType)
    {
        Id = id;
        SellGuid = sellGuid;
        Description = description;
        Amount = amount;
        Date = date;
        MoneyMovementType = moneyMovementType;
    }

    public static MoneyMovement Create(string description, decimal amount,MoneyMovementType movementType, Guid sellGuid = new Guid())
    {
        return new(Guid.NewGuid(), description, amount, DateOnly.FromDateTime(DateAndTime.Now), sellGuid, movementType);
    }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public MoneyMovement() { }

#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
