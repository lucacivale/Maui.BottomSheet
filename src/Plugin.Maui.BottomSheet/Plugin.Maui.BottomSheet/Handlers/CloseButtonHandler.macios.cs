namespace Plugin.Maui.BottomSheet.Handlers;

using Microsoft.Maui.Handlers;
using UIKit;

/// <inheritdoc/>
public class CloseButtonHandler : ButtonHandler
{
    /// <inheritdoc/>
    protected override UIButton CreatePlatformView()
    {
        return new UIButton(UIButtonType.Close);
    }
}