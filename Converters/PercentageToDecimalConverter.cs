using System.Globalization;
using System.Windows.Data;
using static System.Net.Mime.MediaTypeNames;

namespace EstrellaAccesoriosWpf.Converters
{
    internal class PercentageToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                // Format the decimal value as "% 0.00"
                return string.Format(culture, "% {0:N2}", decimalValue);
            }

            return string.Format(culture, "% {0:N2}", 0m);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string Text)
            {
                // Remove the '%' sign and spaces, then parse the string as a decimal

                string temp = Text.Replace("%", "").Replace(",", "").Replace(".", "").Replace(" ", "");

                while (temp.Length > 0 && temp[0] == '0') temp = temp.Remove(0, 1);
                if (temp.Length == 0) temp = "0";

                if (decimal.TryParse(temp, NumberStyles.Any, culture, out decimal result))
                {
                    return result/100;
                }
            }

            return 0m; // Return 0m or handle invalid input as needed
        }
    }
}
