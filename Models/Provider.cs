using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class Provider : ObservableObject
{
    public Guid Id { get; set; }

    [ObservableProperty]
    private string name;

    private Provider(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Provider Create(string name)
    {
        return new(Guid.NewGuid(), name);
    }
    public void Update(Provider provider)
    {
        Name = provider.Name;
    }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    private Provider() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
