using EstrellaAccesoriosWpf.ViewModels;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace EstrellaAccesoriosWpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel, ISnackbarMessageQueue snackbarMessageQueue)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += MainWindow_Loaded;


        var messageQueue = (SnackbarMessageQueue)snackbarMessageQueue;
        MainSnackbar.MessageQueue = messageQueue;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.LoadCommand.Execute(this);
        }
    }

    private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        //until we had a StaysOpen flag to Drawer, this will help with scroll bars
        var dependencyObject = Mouse.Captured as DependencyObject;

        while (dependencyObject != null)
        {
            if (dependencyObject is ScrollBar) return;
            dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
        }

        MenuToggleButton.IsChecked = false;
    }

    private async void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void OnCopy(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is string stringValue)
        {
            try
            {
                Clipboard.SetDataObject(stringValue);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
    }

    private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        => ItemsSearchBox.Focus();

    private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
        => ModifyTheme(DarkModeToggleButton.IsChecked == true);

    private void FlowDirectionButton_Click(object sender, RoutedEventArgs e)
        => FlowDirection = FlowDirectionToggleButton.IsChecked.GetValueOrDefault(false)
            ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;

    private static void ModifyTheme(bool isDarkTheme)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();

        theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
        paletteHelper.SetTheme(theme);
    }

    private void OnSelectedItemChanged(object sender, DependencyPropertyChangedEventArgs e)
        => MainScrollViewer.ScrollToHome();

    
}