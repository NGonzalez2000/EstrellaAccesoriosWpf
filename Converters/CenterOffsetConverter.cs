using System.Globalization;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.Converters;

public class CenterOffsetConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double containerSize && parameter is string popupSizeStr && double.TryParse(popupSizeStr, out double popupSize))
        {
            return (containerSize - popupSize) / 2;
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
