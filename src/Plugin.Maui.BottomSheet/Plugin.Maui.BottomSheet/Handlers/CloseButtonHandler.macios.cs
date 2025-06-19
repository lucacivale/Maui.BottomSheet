namespace Plugin.Maui.BottomSheet.Handlers;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

// ReSharper disable once RedundantNameQualifier
using Plugin.Maui.BottomSheet.Platform.MaciOS;
using UIKit;

/// <summary>
/// Handler responsible for managing the native close button view on macOS and iOS platforms,
/// including mapping custom button properties such as <see cref="CloseButton.TintColor"/>.
/// </summary>
public class CloseButtonHandler : ButtonHandler
{
    /// <summary>
    /// The property mapper for <see cref="CloseButton"/>, including the mapping for the TintColor property.
    /// </summary>
    private static readonly IPropertyMapper<CloseButton, IButtonHandler> _closeButtonMapper = new PropertyMapper<CloseButton, IButtonHandler>(Mapper)
    {
        [nameof(CloseButton.TintColor)] = MapTintColor,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButtonHandler"/> class using the default property and command mappers.
    /// </summary>
    public CloseButtonHandler()
        : base(_closeButtonMapper, CommandMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButtonHandler"/> class with a custom property mapper.
    /// </summary>
    /// <param name="mapper">
    /// The <see cref="IPropertyMapper"/> to use for property mapping. If <c>null</c>, the default mapper is used.
    /// </param>
    public CloseButtonHandler(IPropertyMapper? mapper)
        : base(mapper ?? _closeButtonMapper, CommandMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButtonHandler"/> class with custom property and command mappers.
    /// </summary>
    /// <param name="mapper">
    /// The <see cref="IPropertyMapper"/> for property mapping. If <c>null</c>, the default close button mapper is used.
    /// </param>
    /// <param name="commandMapper">
    /// The <see cref="CommandMapper"/> for command mapping. If <c>null</c>, the default command mapper is used.
    /// </param>
    public CloseButtonHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? _closeButtonMapper, commandMapper ?? CommandMapper)
    {
    }

    /// <summary>
    /// Creates and returns the native <see cref="UIButton"/> for the close button.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="UIButton"/> configured as a close button.
    /// </returns>
    protected override UIButton CreatePlatformView()
    {
        return new UIButton(UIButtonType.Close);
    }

    /// <summary>
    /// Maps the <see cref="CloseButton.TintColor"/> property to the underlying native button.
    /// Configures the background color, foreground color, and appearance of the platform button.
    /// </summary>
    /// <param name="handler">The button handler being configured.</param>
    /// <param name="virtualView">The virtual close button view providing property values.</param>
    private static void MapTintColor(IButtonHandler handler, CloseButton virtualView)
    {
        var config = UIButtonConfiguration.FilledButtonConfiguration;
        config.Background.CornerRadius = 100;
        config.Background.BackgroundColor = virtualView.TintColor.ToPlatform();
        config.BaseForegroundColor = UIColor.FromRGB(132, 132, 136);

        handler.PlatformView.Configuration = config;
    }
}