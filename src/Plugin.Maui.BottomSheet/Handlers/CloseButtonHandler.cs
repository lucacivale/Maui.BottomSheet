namespace Plugin.Maui.BottomSheet.Handlers;

/// <summary>
/// Handles the platform-specific implementation and property mapping for the <see cref="CloseButton"/> view
/// within the Plugin.Maui.BottomSheet framework.
/// </summary>
/// <remarks>
/// This handler manages the rendering and behavior of the <see cref="CloseButton"/> on different platforms, providing
/// property mappings, view creation, and event handling mechanisms. It is an internal framework component
/// and works closely with platform-specific implementations and the MAUI `ViewHandler`.
/// </remarks>
internal sealed partial class CloseButtonHandler
{
    /// <summary>
    /// A static read-only property mapper that maps properties of the <see cref="CloseButton"/> class
    /// to corresponding logic in the <see cref="CloseButtonHandler"/> class.
    /// This mapper defines the mapping behavior for properties like TintColor, HeightRequest,
    /// and WidthRequest, enabling platform-specific handling of these properties.
    /// </summary>
    private static readonly IPropertyMapper<CloseButton, CloseButtonHandler> _closeButtonMapper = new PropertyMapper<CloseButton, CloseButtonHandler>(ElementMapper)
    {
        [nameof(CloseButton.TintColor)] = MapTintColor,
        [nameof(VisualElement.HeightProperty)] = MapHeightRequest,
        [nameof(VisualElement.WidthRequest)] = MapWidthRequest,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButtonHandler"/> class.
    /// Handles the behavior and properties of the CloseButton control across various platforms.
    /// </summary>
    /// <remarks>
    /// The <see cref="CloseButtonHandler"/> class manages the mapping of properties
    /// and commands to their corresponding platform-specific implementations.
    /// It defines methods to map key properties such as TintColor, HeightRequest,
    /// and WidthRequest between the cross-platform CloseButton control and its
    /// platform-specific representation.
    /// </remarks>
    /// <param name="mapper">The property mapper for the <see cref="CloseButtonHandler"/>.</param>
    /// <param name="commandMapper">The command mapper for the <see cref="CloseButtonHandler"/>.</param>
    public CloseButtonHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? _closeButtonMapper, commandMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButtonHandler"/> class.
    /// Handles the functionality and property mapping for the CloseButton control.
    /// </summary>
    /// <remarks>
    /// This handler is responsible for mapping properties and commands related to the <see cref="CloseButton"/> view.
    /// It integrates with the platform-specific implementations and allows customization through property mappers and command mappers.
    /// </remarks>
    public CloseButtonHandler()
        : base(_closeButtonMapper)
    {
    }
}