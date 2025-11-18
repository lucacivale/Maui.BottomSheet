namespace Plugin.BottomSheet.IOSMacCatalyst;

using UIKit;

/// <summary>
/// Provides extension methods for UISheetPresentationControllerDetent collections.
/// </summary>
internal static class UISheetPresentationControllerDetentsExtensions
{
    /// <summary>
    /// Gets the largest detent identifier from a collection of detents.
    /// </summary>
    /// <param name="detents">The collection of detents to analyze.</param>
    /// <returns>The identifier of the largest detent in the collection.</returns>
    public static UISheetPresentationControllerDetentIdentifier LargestDetentIdentifier(this IEnumerable<UISheetPresentationControllerDetent> detents)
    {
        var largestDetent = detents.Last();
        var largestDetentIdentifier = UISheetPresentationControllerDetentIdentifier.Unknown;

        using var mediumDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
        using var largeDetent = UISheetPresentationControllerDetent.CreateLargeDetent();

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

    public static List<BottomSheetState> ToBottomSheetStates(this UISheetPresentationControllerDetent[] detents)
    {
        return detents.Select(x => x.ToBottomSheetState()).ToList();
    }

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