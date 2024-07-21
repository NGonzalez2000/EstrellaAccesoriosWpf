using System.Windows.Controls;
using System.Windows;
using System.Globalization;

namespace EstrellaAccesoriosWpf.Common.Controls;

public class NumericTextBox : TextBox
{
    public NumericTextBox()
    {
        Style = (Style)Application.Current.Resources["MaterialDesignTextBox"];
        TextAlignment = TextAlignment.Right;
        PreviewTextInput += NumericTextBox_PreviewTextInput;
        TextChanged += NumericTextBox_TextChanged;
        GotFocus += NumericTextBox_GotFocus;
    }

    private void NumericTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        CaretIndex = Text.Length + 1;
    }

    private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextChanged -= NumericTextBox_TextChanged;
        bool v = int.TryParse(Text, out int result);
        if(v)
        {
            Text = result.ToString();
        }
        else
        {
            Text = "0";
        }
        if(Text.Length == 0)
        {
            Text = "0";
        }
        CaretIndex = Text.Length;
        TextChanged += NumericTextBox_TextChanged;
    }

    private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = true;
        if(!char.IsNumber(e.Text[0]))
        {
            return;
        }
        Text += e.Text[0];
    }
}
