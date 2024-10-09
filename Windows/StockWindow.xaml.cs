using System.Windows;
using EstrellaAccesoriosWpf.Models;
using System.Windows.Threading;
using EstrellaAccesoriosWpf.ViewModels;
using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Windows;

/// <summary>
/// Lógica de interacción para StockWindow.xaml
/// </summary>
public partial class StockWindow : Window
{
    private bool isIndicatorVisible = true;
    private DispatcherTimer _inputTimer;

    private bool closedFromViewModel = false;
    private TaskCompletionSource<StockIncome> taskCompletionSource = null!;
    public StockWindow(StockWindowViewModel vm)
    {
        InitializeComponent();
        vm.SetAction(CloseInvocation);
        DataContext = vm;
        IsVisibleChanged += StockWindow_IsVisibleChanged;
        _inputTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _inputTimer.Tick += InputTimer_Tick;
        tb_Scanner.Focus();
    }

    public void SetReturnTask(TaskCompletionSource<StockIncome> task)
    {
        taskCompletionSource = task;
        if (DataContext is StockWindowViewModel vm)
        {
            vm.SetReturnTask(task);
        }
    }

    private void CloseInvocation()
    {
        closedFromViewModel = true;
        Close();
    }

    private void StockWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue && DataContext is StockWindowViewModel viewModel)
        {
            viewModel.LoadCommand.Execute(null);
        }
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        if (!closedFromViewModel && taskCompletionSource is not null && !taskCompletionSource.Task.IsCanceled)
        {
            taskCompletionSource.SetCanceled();
        }
        this.Hide();
    }

    private void Btn_Scanner_Click(object sender, RoutedEventArgs e)
    {
        isIndicatorVisible = true;
        ChangeScannState();
        tb_Scanner.Focus();
    }

    private void ChangeScannState()
    {
        MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btn_Scanner, isIndicatorVisible);
        btn_Scanner.Content = isIndicatorVisible ? "ESCANEANDO" : "ESCANEAR";
    }

    private void Tb_Scanner_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textbox) return;
        _inputTimer.Stop();
        if (string.IsNullOrEmpty(textbox.Text))
        {
            return;
        }
        _inputTimer.Start();
    }

    private void Tb_Scanner_LostFocus(object sender, RoutedEventArgs e)
    {
        isIndicatorVisible = false;
        ChangeScannState();
    }

    private void InputTimer_Tick(object? sender, EventArgs e)
    {
        string txt = tb_Scanner.Text;

        if (DataContext is StockWindowViewModel vm)
        {
            vm.AddStockItem(txt);
        }
        tb_Scanner.Text = string.Empty;
    }
   
}
