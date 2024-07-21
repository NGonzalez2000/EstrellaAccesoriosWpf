using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;
using System.Windows.Controls;
namespace EstrellaAccesoriosWpf.Views;
public partial class HomeView : View
{
    public HomeView(HomeViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}
