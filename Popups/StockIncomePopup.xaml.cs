using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.Popups
{
    /// <summary>
    /// Lógica de interacción para StockIncomePopup.xaml
    /// </summary>
    public partial class StockIncomePopup : Popup
    {
        public StockIncomePopup(StockIncome vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        public async Task View()
        {
            SetHeader("INGRESO");
            await DialogHost.Show(this, "RootDialog");
        }
    }
}
