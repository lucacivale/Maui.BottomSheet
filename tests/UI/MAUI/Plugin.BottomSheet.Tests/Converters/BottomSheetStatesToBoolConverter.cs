using System.Globalization;

namespace Plugin.BottomSheet.Tests.Converters;

public class BottomSheetStatesToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        IEnumerable<BottomSheetState> states = (IEnumerable<BottomSheetState>)value!;

        return states.Contains((BottomSheetState)parameter!);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}