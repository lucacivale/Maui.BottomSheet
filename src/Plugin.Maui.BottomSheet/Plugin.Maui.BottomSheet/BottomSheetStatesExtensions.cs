namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Bottom sheet states collection extensions
/// </summary>
public static class BottomSheetStatesExtensions
{
    /// <summary>
    /// Is bottom sheet state allowed.
    /// </summary>
    /// <param name="states">Allowed states.</param>
    /// <param name="state">State to check.</param>
    /// <returns>Is state allowed.</returns>
    public static bool IsStateAllowed(this IEnumerable<BottomSheetState> states, BottomSheetState state)
    {
        return states.Contains(state);
    }
}