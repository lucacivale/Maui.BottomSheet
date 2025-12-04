using Microsoft.UI.Xaml.Controls;

namespace Plugin.BottomSheet.Windows;

/// <summary>
/// Represents a custom button for closing the bottom sheet, using a cancel icon as its content.
/// The button's size is automatically adjusted based on its minimum width and height.
/// </summary>
internal sealed partial class CloseButton : Button
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButton"/> class.
    /// Sets the content of the button to a cancel icon.
    /// </summary>
    public CloseButton()
    {
        Content = new SymbolIcon(Symbol.Cancel);
    }

    /// <summary>
    /// Sets the minimum height of the close button and adjusts the icon size accordingly.
    /// </summary>
    /// <param name="height">The minimum height of the button.</param>
    public void SetHeight(double height)
    {
        MinHeight = height;

        SetIconSize();
    }

    /// <summary>
    /// Sets the minimum width of the close button and adjusts the icon size accordingly.
    /// </summary>
    /// <param name="width">The minimum width of the button.</param>
    public void SetWidth(double width)
    {
        MinWidth = width;

        SetIconSize();
    }

    /// <summary>
    /// Adjusts the size of the cancel icon based on the current button's minimum width and height.
    /// The icon size is set to half the sum of the button's minimum width and height.
    /// </summary>
    private void SetIconSize()
    {
        FontSize = Convert.ToInt32((MinHeight + MinWidth) / 2);
    }
}
