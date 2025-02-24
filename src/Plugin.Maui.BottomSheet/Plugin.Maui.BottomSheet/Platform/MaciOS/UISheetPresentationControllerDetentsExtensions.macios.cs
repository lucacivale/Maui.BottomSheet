namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using UIKit;

/// <summary>
/// UISheetPresentationControllerDetent collection extension methods.
/// </summary>
internal static class UISheetPresentationControllerDetentsExtensions
{
    /// <summary>
    /// Get the largest detent identifier of the collection.
    /// </summary>
    /// <param name="detents">Detent collection.</param>
    /// <returns>Largest detent.</returns>
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