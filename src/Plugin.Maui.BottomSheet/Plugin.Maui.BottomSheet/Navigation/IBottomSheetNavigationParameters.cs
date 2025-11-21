using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Represents a collection of parameters used to pass data during BottomSheet navigation.
/// </summary>
/// <remarks>
/// This interface provides a mechanism for passing key-value pairs as parameters
/// to facilitate communication between the components involved in BottomSheet navigation.
/// It extends the <see cref="IDictionary{TKey, TValue}"/> interface, where the key is a string
/// and the value is an object.
/// </remarks>
[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is a collection.")]
public interface IBottomSheetNavigationParameters : IDictionary<string, object>;