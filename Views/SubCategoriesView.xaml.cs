using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.ViewModels;
using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Views
{
    /// <summary>
    /// Lógica de interacción para SubCategoriesView.xaml
    /// </summary>
    public partial class SubCategoriesView : View
    {
        public SubCategoriesView(SubCategoryViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}
