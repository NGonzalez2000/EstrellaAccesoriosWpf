using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EstrellaAccesoriosWpf.Popups
{
    /// <summary>
    /// Lógica de interacción para PaymentMethodPopup.xaml
    /// </summary>
    public partial class PaymentMethodPopup : Popup
    {
        public PaymentMethodPopup(PaymentMethod paymentMethod)
        {
            InitializeComponent();
            DataContext = paymentMethod;
        }
        public async Task<bool> Create()
        {
            SetHeader("CREAR MEDIO DE PAGO");
            object? response = await DialogHost.Show(this, "RootDialog");
            if (response is bool b && b)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Update()
        {
            SetHeader("EDITAR MEDIO DE PAGO");
            object? response = await DialogHost.Show(this, "RootDialog");
            if (response is bool b && b)
            {
                return true;
            }
            return false;
        }
    }
}
