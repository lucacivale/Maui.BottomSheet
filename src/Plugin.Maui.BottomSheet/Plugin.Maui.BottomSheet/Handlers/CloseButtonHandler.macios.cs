namespace Plugin.Maui.BottomSheet.Handlers;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

// ReSharper disable once RedundantNameQualifier
using Plugin.Maui.BottomSheet.Platform.MaciOS;
using UIKit;

/// <inheritdoc/>
public class CloseButtonHandler : ButtonHandler
{
    // ReSharper disable once ArrangeModifiersOrder
    private static readonly IPropertyMapper<CloseButton, IButtonHandler> _closeButtonMapper = new PropertyMapper<CloseButton, IButtonHandler>(Mapper)
    {
        [nameof(CloseButton.TintColor)] = MapTintColor,
    };

    public CloseButtonHandler()
        : base(_closeButtonMapper, CommandMapper)
    {
    }

    public CloseButtonHandler(IPropertyMapper? mapper)
        : base(mapper ?? _closeButtonMapper, CommandMapper)
    {
    }

    public CloseButtonHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? _closeButtonMapper, commandMapper ?? CommandMapper)
    {
    }

    /// <inheritdoc/>
    protected override UIButton CreatePlatformView()
    {
        return new UIButton(UIButtonType.Close);
    }

    private static void MapTintColor(IButtonHandler handler, CloseButton virtualView)
    {
        var config = UIButtonConfiguration.FilledButtonConfiguration;
        config.Background.CornerRadius = 100;
        config.Background.BackgroundColor = virtualView.TintColor.ToPlatform();
        config.BaseForegroundColor = UIColor.FromRGB(132, 132, 136);

        handler.PlatformView.Configuration = config;
    }
}