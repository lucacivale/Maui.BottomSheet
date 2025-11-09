using System.Globalization;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.Converters;

public class StringToThicknessConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str
            && string.IsNullOrWhiteSpace(str))
        {
            return Thickness.Zero;
        }
        return new Thickness(System.Convert.ToDouble(value));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}