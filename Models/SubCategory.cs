using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class SubCategory : ObservableObject
{
    public Guid Id { get; set; }
    [ObservableProperty]
    public string description;
    private SubCategory(Guid id, string description)
    {
        Id = id;
        Description = description;
    }
    public static SubCategory Create(string description)
    {
        return new SubCategory(Guid.NewGuid(), description);
    }
    public void Update(SubCategory subCategory) 
    {
        Description = subCategory.Description;
    }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public SubCategory() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
