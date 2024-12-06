namespace Plugin.Maui.BottomSheet;

using System.ComponentModel;
using System.Globalization;

/// <summary>
/// Convert a comma seperated list of <see cref="BottomSheetState"/> values to a <see cref="ICollection{T}"/>.
/// </summary>
public class StringToBottomSheetStateTypeConverter : TypeConverter
{
    /// <inheritdoc/>
    // ReSharper disable once ArrangeModifiersOrder
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
        => sourceType == typeof(string);

    /// <inheritdoc/>
    // ReSharper disable once ArrangeModifiersOrder
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(ICollection<BottomSheetState>);

    /// <inheritdoc/>
    // ReSharper disable once ArrangeModifiersOrder
    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string stringValue
            || string.IsNullOrWhiteSpace(stringValue))
        {
            return Enumerable.Empty<BottomSheetState>().ToList();
        }

        stringValue = stringValue.Trim();

        return stringValue.Split(',').Select(Enum.Parse<BottomSheetState>).ToList();
    }

    /// <inheritdoc/>
    // ReSharper disable once ArrangeModifiersOrder
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value is not ICollection<BottomSheetState> states ? null : string.Join(",", states);
    }
}