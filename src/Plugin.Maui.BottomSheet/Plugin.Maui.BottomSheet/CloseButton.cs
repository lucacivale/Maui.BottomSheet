namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Represents a custom button control used for closing a bottom sheet or similar UI component.
/// </summary>
internal sealed class CloseButton : View
{
    /// <summary>
    /// Identifies the bindable property for the tint color of the CloseButton.
    /// This property defines the color that will be applied to the CloseButton icon
    /// within the bottom sheet header. The tint color can be customized through bindings
    /// or set directly to match the visual style of the application.
    /// </summary>
    public static readonly BindableProperty TintProperty =
        BindableProperty.Create(
            nameof(TintColor),
            typeof(Color),
            typeof(CloseButton));

    private readonly WeakEventManager _weakEventManager = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButton"/> class.
    /// Represents a button designed to handle close actions within a bottom sheet control.
    /// </summary>
    /// <param name="horizontalOptions">The horizontal layout options for the close button.</param>
    /// <remarks>
    /// This button provides a configurable tint color and supports a clicked event to handle user interactions.
    /// It is primarily used within the bottom sheet component of the Plugin.Maui.BottomSheet namespace.
    /// </remarks>
    public CloseButton(LayoutOptions horizontalOptions)
    {
        HeightRequest = 40;
        WidthRequest = 40;
        HorizontalOptions = horizontalOptions;
    }

    /// <summary>
    /// Occurs when the close button is clicked.
    /// </summary>
    public event EventHandler? Clicked
    {
        add => _weakEventManager.AddEventHandler(value);
        remove => _weakEventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets the tint color for the close button.
    /// This property defines the color used to tint the foreground or background
    /// elements of the button, such as its icon, on supported platforms.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> representing the tint color. If null, the default
    /// platform-specific appearance will be used.
    /// </value>
    public Color? TintColor
    {
        get => (Color)GetValue(TintProperty);
        set => SetValue(TintProperty, value);
    }

    /// <summary>
    /// Triggers the <see cref="Clicked"/> event, notifying subscribers that the close button has been clicked.
    /// This method is used internally by the <see cref="CloseButton"/> and its handlers
    /// to raise the event when the button interaction is detected.
    /// </summary>
    internal void RaiseClicked()
    {
        _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(Clicked));
    }
}