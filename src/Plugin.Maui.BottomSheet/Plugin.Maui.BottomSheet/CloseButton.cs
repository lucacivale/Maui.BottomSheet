namespace Plugin.Maui.BottomSheet;

internal sealed class CloseButton : View
{
    public static readonly BindableProperty TintProperty =
        BindableProperty.Create(
            nameof(TintColor),
            typeof(Color),
            typeof(CloseButton));

    private readonly WeakEventManager _weakEventManager = new();

    public CloseButton(LayoutOptions horizontalOptions)
    {
        HeightRequest = 40;
        WidthRequest = 40;
        HorizontalOptions = horizontalOptions;
    }

    public event EventHandler? Clicked
    {
        add => _weakEventManager.AddEventHandler(value);
        remove => _weakEventManager.RemoveEventHandler(value);
    }

    public Color? TintColor { get => (Color)GetValue(TintProperty); set => SetValue(TintProperty, value); }

    internal void RaiseClicked()
    {
        _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(Clicked));
    }
}