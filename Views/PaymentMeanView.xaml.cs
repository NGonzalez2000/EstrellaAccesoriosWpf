using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;
using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Views;

/// <summary>
/// Lógica de interacción para PaymentMeanView.xaml
/// </summary>
public partial class PaymentMeanView : View
{
    public PaymentMeanView(PaymentMeanViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}
