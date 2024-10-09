using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class CashClose : ObservableObject
{
    public Guid Id { get; set; }
    public DateOnly Date {  get; set; }

    [ObservableProperty]
    private decimal balance;

    private CashClose(Guid id , DateOnly date, decimal balance)
    {
        Id = id;
        Date = date;
        Balance = balance;
    }

    public static CashClose Create(DateOnly date, decimal balance)
    {
        return new(Guid.NewGuid(), date, balance);
    }

    public void AddBalance(decimal amount)
    {
        Balance += amount;
    }

    public void RemoveBalance(decimal amount)
    {
        Balance -= amount;
    }
}
