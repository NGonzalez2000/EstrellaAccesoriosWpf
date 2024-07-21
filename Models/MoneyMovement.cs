using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;

namespace EstrellaAccesoriosWpf.Models;

public partial class MoneyMovement : ObservableObject
{
    public Guid Id { get; set; }
    [ObservableProperty]
    private string description;
    [ObservableProperty]
    private decimal amount;
    [ObservableProperty]
    private DateOnly date;

    private MoneyMovement(Guid id, string description, decimal amount, DateOnly date)
    {
        Id = id;
        Description = description;
        Amount = amount;
        Date = date;
    }

    public static MoneyMovement Create(string description, decimal amount)
    {
        return new(Guid.NewGuid(), description, amount, DateOnly.FromDateTime(DateAndTime.Now));
    }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public MoneyMovement() { }

#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
