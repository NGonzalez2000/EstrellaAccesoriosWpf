using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Common;

public partial class TabbedPage(View view) : ObservableObject
{
    private readonly View? _content = view;
    [ObservableProperty]
    private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto;
    [ObservableProperty]
    private ScrollBarVisibility _verticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto;
    [ObservableProperty]
    private Thickness _marginRequirement = new(0);
    public string Name { get; } = view.Title;
    public View? Content => _content;
}
