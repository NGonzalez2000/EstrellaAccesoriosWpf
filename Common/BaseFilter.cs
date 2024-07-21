using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace EstrellaAccesoriosWpf.Common;

public abstract partial class BaseFilter() : ObservableObject
{
    [ObservableProperty]
    private string selectedOption = string.Empty;

    [ObservableProperty]
    private string selectedDirection = string.Empty;

    [ObservableProperty]
    private List<string> sortOptions = [];

    [ObservableProperty]
    private List<string> sortDirection = [ "Ascendente", "Descendente" ];

    protected Dictionary<string, string> PropertyMapping = [];

    public abstract bool Filter(object o);

    public abstract SortDescription GetSortDescription();
}
