using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;

namespace EstrellaAccesoriosWpf.Views;

/// <summary>
/// Lógica de interacción para DailyBoxView.xaml
/// </summary>
public partial class DailyBoxView : View
{
    public DailyBoxView(DailyBoxViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}
