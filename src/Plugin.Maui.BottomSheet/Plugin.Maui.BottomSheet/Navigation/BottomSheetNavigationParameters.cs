namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Represents the parameters used for BottomSheet navigation operations.
/// </summary>
public class BottomSheetNavigationParameters : Dictionary<string, object>, IBottomSheetNavigationParameters
{
    /// <summary>
    /// Creates an empty instance of <see cref="BottomSheetNavigationParameters"/>.
    /// </summary>
    /// <returns>A new, empty <see cref="BottomSheetNavigationParameters"/> object.</returns>
    public static BottomSheetNavigationParameters Empty() => new();
}