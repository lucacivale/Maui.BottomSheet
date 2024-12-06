// ReSharper disable once CheckNamespace
namespace Plugin.Maui.BottomSheet.Platforms.Android;

using Microsoft.Maui.Platform;

/// <summary>
/// <see cref="ContainerView"/> extension methods.
/// </summary>
public static class ContainerViewExtensions
{
    /// <summary>
    /// Set padding.
    /// </summary>
    /// <param name="containerView"><see cref="ContainerView"/>.</param>
    /// <param name="padding"><see cref="Thickness"/> with pixel values.</param>
    public static void SetPadding(this ContainerView? containerView, Thickness padding)
    {
        containerView?.SetPadding(
            Convert.ToInt32(padding.Left),
            Convert.ToInt32(padding.Top),
            Convert.ToInt32(padding.Right),
            Convert.ToInt32(padding.Bottom));
    }
}