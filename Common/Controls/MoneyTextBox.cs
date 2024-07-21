using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Common.Controls;

public class MoneyTextBox : TextBox
{
    
    public MoneyTextBox()
    {
        Style = (Style)Application.Current.Resources["MaterialDesignTextBox"];
        TextAlignment = TextAlignment.Right;
        Text = 0m.ToString("C2", CultureInfo.CurrentCulture);
        PreviewTextInput += MoneyTextBox_PreviewTextInput;
        TextChanged += MoneyTextBox_TextChanged;
    }

    private void MoneyTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        CaretIndex = int.MaxValue;
    }

    private void MoneyTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = true;
        if (char.IsNumber(e.Text[0]))
        {
            ((MoneyTextBox)sender).Text += e.Text[0];
        }
    }
}
