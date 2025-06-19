namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

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

        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
#pragma warning disable CA1416
            if (largestDetent.Identifier != BottomSheetUIViewController.PeekDetentId)
#pragma warning restore CA1416
            {
                largestDetentIdentifier = LargestDetentIdentifier(largestDetent);
            }
        }
        else
        {
            largestDetentIdentifier = LargestDetentIdentifier(largestDetent);
        }

        return largestDetentIdentifier;
    }

    /// <summary>
    /// Determines the detent identifier for a given detent by comparing with system detents.
    /// </summary>
    /// <param name="largestDetent">The detent to identify.</param>
    /// <returns>The corresponding detent identifier (Medium or Large).</returns>
    private static UISheetPresentationControllerDetentIdentifier LargestDetentIdentifier(UISheetPresentationControllerDetent largestDetent)
    {
        using var mediumDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
        UISheetPresentationControllerDetentIdentifier largestDetentIdentifier;

        if (largestDetent.Equals(mediumDetent))
        {
            largestDetentIdentifier = UISheetPresentationControllerDetentIdentifier.Medium;
        }
        else
        {
            largestDetentIdentifier = UISheetPresentationControllerDetentIdentifier.Large;
        }

        return largestDetentIdentifier;
    }
}