namespace Plugin.Maui.BottomSheet.Handlers;

using Microsoft.Maui.Handlers;
using UIKit;

/// <inheritdoc/>
public class CloseButtonHandler : ButtonHandler
{
    protected override UIButton CreatePlatformView()
    {
        return new UIButton(UIButtonType.Close);
    }
}