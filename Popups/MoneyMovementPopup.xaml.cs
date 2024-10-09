using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.Popups
{
    /// <summary>
    /// Lógica de interacción para MoneyMovementPopup.xaml
    /// </summary>
    public partial class MoneyMovementPopup : Popup
    {
        private List<MoneyMovementType> operations = [];
        public MoneyMovementPopup(MoneyMovement moneyMovement, EstrellaDbContext dbContext)
        {
            InitializeComponent();
            DataContext = moneyMovement;
            operations = dbContext.MoneyMovementTypes.Where(x => x.Description != "VENTA").ToList();
            Cb_Operation.ItemsSource = operations;
            Cb_Operation.SelectedIndex = 0;
        }
        public async Task<bool> Create()
        {
            SetHeader("CREAR MOVIMIENTO DE CAJA");
            object? response = await DialogHost.Show(this, "RootDialog");
            if (response is bool b && b)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Update()
        {
            SetHeader("EDITAR MOVIMIENTO DE CAJA");
            object? response = await DialogHost.Show(this, "RootDialog");
            if (response is bool b && b)
            {
                return true;
            }
            return false;
        }

        private void Cb_Operation_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(DataContext is MoneyMovement moneyMovement)
            {
                moneyMovement.MoneyMovementType = (MoneyMovementType)Cb_Operation.SelectedItem;
            }
        }
    }
}
