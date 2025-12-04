using Plugin.BottomSheet;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Provides a set of extension methods for interaction with collections of <see cref="BottomSheetState"/>,
/// enabling functionalities like checking if a state is allowed within a given set of allowed states.
/// </summary>
internal static class BottomSheetStatesExtensions
{
    /// <summary>
    /// Determines whether the specified state is included in the provided collection of bottom sheet states.
    /// </summary>
    /// <param name="states">The collection of bottom sheet states to evaluate.</param>
    /// <param name="state">The bottom sheet state to check for inclusion.</param>
    /// <returns>True if the state is included in the collection, false otherwise.</returns>
    public static bool IsStateAllowed(this IEnumerable<BottomSheetState> states, BottomSheetState state)
    {
        return states.Contains(state);
    }
}