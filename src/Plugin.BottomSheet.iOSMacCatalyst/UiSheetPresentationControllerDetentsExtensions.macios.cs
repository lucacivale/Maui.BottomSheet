namespace Plugin.BottomSheet.iOSMacCatalyst;

using UIKit;

/// <summary>
/// Contains extension methods for working with UISheetPresentationControllerDetent instances
/// and their conversions to BottomSheetState values.
/// </summary>
internal static class UiSheetPresentationControllerDetentsExtensions
{
    /// <summary>
    /// Gets the largest detent identifier from a collection of detents.
    /// </summary>
    /// <param name="detents">The collection of detents to analyze.</param>
    /// <returns>The identifier of the largest detent in the collection.</returns>
    public static UISheetPresentationControllerDetentIdentifier LargestDetentIdentifier(
        this IEnumerable<UISheetPresentationControllerDetent> detents)
    {
        UISheetPresentationControllerDetent largestDetent = detents.Last();
        UISheetPresentationControllerDetentIdentifier largestDetentIdentifier = UISheetPresentationControllerDetentIdentifier.Unknown;

        using UISheetPresentationControllerDetent mediumDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
        using UISheetPresentationControllerDetent largeDetent = UISheetPresentationControllerDetent.CreateLargeDetent();

        if (largestDetent.Equals(mediumDetent))
        {
            largestDetentIdentifier = UISheetPresentationControllerDetentIdentifier.Medium;
        }
        else if (largestDetent.Equals(largeDetent))
        {
            largestDetentIdentifier = UISheetPresentationControllerDetentIdentifier.Large;
        }

        return largestDetentIdentifier;
    }

    /// <summary>
    /// Converts an array of UISheetPresentationControllerDetent objects to a list of BottomSheetState values.
    /// </summary>
    /// <param name="detents">The array of UISheetPresentationControllerDetent objects to convert.</param>
    /// <returns>A list of BottomSheetState values representing the states corresponding to the provided detents.</returns>
    public static List<BottomSheetState> ToBottomSheetStates(this UISheetPresentationControllerDetent[] detents)
    {
        return detents.Select(x => x.ToBottomSheetState()).ToList();
    }

    /// <summary>
    /// Converts the specified detent to its corresponding bottom sheet state.
    /// </summary>
    /// <param name="detent">The detent to convert.</param>
    /// <returns>The corresponding <see cref="BottomSheetState"/> representing the level of expansion or visibility of the bottom sheet.</returns>
    public static BottomSheetState ToBottomSheetState(this UISheetPresentationControllerDetent detent)
    {
        using UISheetPresentationControllerDetent mediumDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
        using UISheetPresentationControllerDetent largeDetent = UISheetPresentationControllerDetent.CreateLargeDetent();

        BottomSheetState state = BottomSheetState.Peek;

        if (detent.Equals(mediumDetent))
        {
            state = BottomSheetState.Medium;
        }
        else if (detent.Equals(largeDetent))
        {
            state = BottomSheetState.Large;
        }

        return state;
    }
}