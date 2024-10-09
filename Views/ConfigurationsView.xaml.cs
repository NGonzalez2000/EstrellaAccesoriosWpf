using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;

namespace EstrellaAccesoriosWpf.Views;

/// <summary>
/// Lógica de interacción para ConfigurationsView.xaml
/// </summary>
public partial class ConfigurationsView : View
{
    public ConfigurationsView(ConfigurationViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}
