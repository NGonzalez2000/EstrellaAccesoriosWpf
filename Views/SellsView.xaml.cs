using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;

namespace EstrellaAccesoriosWpf.Views
{
    /// <summary>
    /// Lógica de interacción para SellsView.xaml
    /// </summary>
    public partial class SellsView : View
    {
        public SellsView(SellsViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}
