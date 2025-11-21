namespace Plugin.BottomSheet;

/// <summary>
/// Provides extension methods for handling operations related to bottom sheet states.
/// </summary>
internal static class BottomSheetStatesExtensions
{
    /// <summary>
    /// Determines whether the specified state is allowed in the collection of bottom sheet states.
    /// </summary>
    /// <param name="states">The collection of allowed bottom sheet states.</param>
    /// <param name="state">The bottom sheet state to check for allowance.</param>
    /// <returns>True if the bottom sheet state is allowed, false otherwise.</returns>
    public static bool IsStateAllowed(this IEnumerable<BottomSheetState> states, BottomSheetState state)
    {
        return states.Contains(state);
    }
}