using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;

namespace EstrellaAccesoriosWpf.Views
{
    /// <summary>
    /// Lógica de interacción para TicketView.xaml
    /// </summary>
    public partial class TicketView : View
    {
        public TicketView(TicketViewModel vm) : base(vm)
        {
            InitializeComponent();
            MakeViewOnlyMainContent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            products.SelectedItems.Clear();
            stockItems.SelectedItems.Clear();
        }
    }
}
