using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class MoneyMovementType : ObservableObject
{
    public Guid Id { get; set; }

    [ObservableProperty]
    private string description;

    private MoneyMovementType(Guid id, string description)
    {
        Id = id;
        Description = description;
    }

    public static MoneyMovementType Create(string description)
    {
        return new(Guid.NewGuid(), description);
    }
}

