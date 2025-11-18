namespace Plugin.Maui.BottomSheet;

using Plugin.BottomSheet;
using System.ComponentModel;
using System.Globalization;

/// <summary>
/// Type converter that converts a comma-separated string of bottom sheet state values to a collection of BottomSheetState enums and vice versa.
/// </summary>
public sealed class StringToBottomSheetStateTypeConverter : TypeConverter
{
    /// <summary>
    /// Determines whether this converter can convert from the specified source type.
    /// </summary>
    /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
    /// <param name="sourceType">A Type that represents the type you want to convert from.</param>
    /// <returns>True if this converter can perform the conversion; otherwise, false.</returns>
    // ReSharper disable once ArrangeModifiersOrder
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
        => sourceType == typeof(string);

    /// <summary>
    /// Determines whether this converter can convert to the specified destination type.
    /// </summary>
    /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
    /// <param name="destinationType">A Type that represents the type you want to convert to.</param>
    /// <returns>True if this converter can perform the conversion; otherwise, false.</returns>
    // ReSharper disable once ArrangeModifiersOrder
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(ICollection<BottomSheetState>);

    /// <summary>
    /// Converts the given object to the type of this converter, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
    /// <param name="culture">The CultureInfo to use as the current culture.</param>
    /// <param name="value">The Object to convert.</param>
    /// <returns>An Object that represents the converted value.</returns>
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

    /// <summary>
    /// Converts the given value object to the specified type, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
    /// <param name="culture">A CultureInfo. If null is passed, the current culture is assumed.</param>
    /// <param name="value">The Object to convert.</param>
    /// <param name="destinationType">The Type to convert the value parameter to.</param>
    /// <returns>An Object that represents the converted value.</returns>
    // ReSharper disable once ArrangeModifiersOrder
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value is not ICollection<BottomSheetState> states ? null : string.Join(",", states);
    }
}