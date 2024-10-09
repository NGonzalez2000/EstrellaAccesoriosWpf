using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Popups
{
    /// <summary>
    /// Lógica de interacción para WorkInProgressPopup.xaml
    /// </summary>
    public partial class WorkInProgressPopup : UserControl
    {
        public WorkInProgressPopup(string content)
        {
            InitializeComponent();
            operationDisplayed.Text = content;
        }
    }
}
