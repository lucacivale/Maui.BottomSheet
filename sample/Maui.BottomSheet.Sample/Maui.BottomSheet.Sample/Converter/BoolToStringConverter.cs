using System.Globalization;

namespace Maui.BottomSheet.Samples.Converter;
internal class BoolToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((bool)value) == true ? "True" : "False";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((string)value) == "True";
    }
}
