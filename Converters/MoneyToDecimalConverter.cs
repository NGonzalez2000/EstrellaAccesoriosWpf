using System.Globalization;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.Converters;

internal class MoneyToDecimalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is decimal d)
        {
            return d.ToString("C2", CultureInfo.CurrentCulture);
        }
        return 0m.ToString("C2", CultureInfo.CurrentCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string Text) return 0m;

        string temp = Text.Replace("$", "").Replace(",", "").Replace(".", "").Replace(" ", "");

        while (temp.Length > 0 && temp[0] == '0') temp = temp.Remove(0, 1);
        if (temp.Length == 0) temp = "0";

        
        if (decimal.TryParse(temp, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal result))
            return result/100;
        return 0m;
    }

}
