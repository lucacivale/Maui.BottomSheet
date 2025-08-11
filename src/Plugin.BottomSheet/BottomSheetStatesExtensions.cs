namespace Plugin.BottomSheet;

/// <summary>
/// Extension methods for working with bottom sheet states collections.
/// </summary>
public static class BottomSheetStatesExtensions
{
    /// <summary>
    /// Determines whether the specified state is allowed in the collection of states.
    /// </summary>
    /// <param name="states">The collection of allowed states.</param>
    /// <param name="state">The state to check for allowance.</param>
    /// <returns>True if the state is allowed, false otherwise.</returns>
    public static bool IsStateAllowed(this IEnumerable<BottomSheetState> states, BottomSheetState state)
    {
        return states.Contains(state);
    }
}