namespace Plugin.Maui.BottomSheet.Handlers;

internal sealed partial class CloseButtonHandler
{
    private static readonly IPropertyMapper<CloseButton, CloseButtonHandler> _closeButtonMapper = new PropertyMapper<CloseButton, CloseButtonHandler>(ElementMapper)
    {
        [nameof(CloseButton.TintColor)] = MapTintColor,
        [nameof(VisualElement.HeightProperty)] = MapHeightRequest,
        [nameof(VisualElement.WidthRequest)] = MapWidthRequest,
    };

    public CloseButtonHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? _closeButtonMapper, commandMapper)
    {
    }

    public CloseButtonHandler()
        : base(_closeButtonMapper)
    {
    }
}