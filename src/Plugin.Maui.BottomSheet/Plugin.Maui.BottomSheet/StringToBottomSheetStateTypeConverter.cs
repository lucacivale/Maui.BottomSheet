namespace Plugin.Maui.BottomSheet;

using Plugin.BottomSheet;
using System.ComponentModel;
using System.Globalization;

/// <summary>
/// A type converter that facilitates conversion between a string representation of bottom sheet state values
/// and a collection of BottomSheetState enums. This enables binding and declarative configuration of
/// bottom sheet states in XAML or other markup styles.
/// </summary>
public sealed class StringToBottomSheetStateTypeConverter : TypeConverter
{
    /// <summary>
    /// Determines whether this converter can convert from the specified source type.
    /// </summary>
    /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
    /// <param name="sourceType">A Type that represents the type to be evaluated for conversion from.</param>
    /// <returns>True if this converter can perform the conversion from the specified source type; otherwise, false.</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
        => sourceType == typeof(string);

    /// <summary>
    /// Determines whether this converter can convert to the specified destination type.
    /// </summary>
    /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
    /// <param name="destinationType">A Type that represents the type you want to convert to.</param>
    /// <returns>True if this converter can perform the conversion; otherwise, false.</returns>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(ICollection<BottomSheetState>);

    /// <summary>
    /// Converts the given object to the type of this converter, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
    /// <param name="culture">The CultureInfo to use as the current culture.</param>
    /// <param name="value">The object to convert.</param>
    /// <returns>An object that represents the converted value as a collection of BottomSheetState enums,
    /// or an empty collection if the input string is null, empty, or whitespace.</returns>
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
    /// <param name="value">The object to convert.</param>
    /// <param name="destinationType">The Type to convert the value parameter to.</param>
    /// <returns>An object that represents the converted value, or null if the conversion cannot be performed.</returns>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value is not ICollection<BottomSheetState> states ? null : string.Join(",", states);
    }
}