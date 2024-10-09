using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;
using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Views;

/// <summary>
/// Lógica de interacción para PaymentMeanView.xaml
/// </summary>
public partial class PaymentMethodView : View
{
    public PaymentMethodView(PaymentMethodViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}
