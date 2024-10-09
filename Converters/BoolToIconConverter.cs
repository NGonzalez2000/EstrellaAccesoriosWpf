using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.Converters;

public class BoolToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is not bool temp)
        {
            return "X";
        }

        return temp ? PackIconKind.Check : PackIconKind.Close;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
