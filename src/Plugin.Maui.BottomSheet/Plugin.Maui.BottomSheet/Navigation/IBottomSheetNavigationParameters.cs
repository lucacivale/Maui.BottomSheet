using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Represents a collection of key-value pairs used for passing data during bottom sheet navigation.
/// </summary>
[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is a collection.")]
public interface IBottomSheetNavigationParameters : IDictionary<string, object>;