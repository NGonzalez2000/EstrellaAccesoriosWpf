using System.Globalization;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.Converters;

public class DateTimeToDateOnlyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is not DateOnly dateOnly)
        {
            return DateTime.Now;
        }
        return new DateTime(dateOnly, new TimeOnly(0,0,0));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTime dateTime)
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }
        return DateOnly.FromDateTime(dateTime);
    }
}
