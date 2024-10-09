using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace EstrellaAccesoriosWpf.Common.Controls;

internal class PercentageTextBox : TextBox
{
    public PercentageTextBox()
    {
        Style = (Style)Application.Current.Resources["MaterialDesignTextBox"];
        TextAlignment = TextAlignment.Right;
        Text = 0m.ToString("C2", CultureInfo.CurrentCulture);
        PreviewTextInput += PercentageTextBox_PreviewTextInput;
        TextChanged += PercentageTextBox_TextChanged;
    }

    private void PercentageTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        CaretIndex = int.MaxValue;
    }

    private void PercentageTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = true;
        if (char.IsNumber(e.Text[0]))
        {
            ((PercentageTextBox)sender).Text += e.Text[0];
        }
    }
}
