namespace Plugin.Maui.BottomSheet.Handlers;

using Plugin.Maui.BottomSheet;

/// <summary>
/// <see cref="IBottomSheet"/> handler.
/// </summary>
internal sealed partial class BottomSheetHandler
{
    // ReSharper disable once ArrangeModifiersOrder
    private static readonly IPropertyMapper<IBottomSheet, BottomSheetHandler> BottomSheetMapper = new PropertyMapper<IBottomSheet, BottomSheetHandler>(ElementMapper)
    {
        [nameof(IBottomSheet.IsCancelable)] = MapIsCancelable,
        [nameof(IBottomSheet.HasHandle)] = MapHasHandle,
        [nameof(IBottomSheet.ShowHeader)] = MapShowHeader,
        [nameof(IBottomSheet.IsOpen)] = MapIsOpen,
        [nameof(IBottomSheet.IsDraggable)] = MapIsDraggable,
        [nameof(IBottomSheet.Header)] = MapHeader,
        [nameof(IBottomSheet.States)] = MapStates,
        [nameof(IBottomSheet.CurrentState)] = MapCurrentState,
        [nameof(IBottomSheet.Peek)] = MapPeek,
        [nameof(IBottomSheet.Content)] = MapContent,
        [nameof(IBottomSheet.Padding)] = MapPadding,
        [nameof(IBottomSheet.BackgroundColor)] = MapBackgroundColor,
        [nameof(IBottomSheet.IgnoreSafeArea)] = MapIgnoreSafeArea,
        [nameof(IBottomSheet.CornerRadius)] = MapCornerRadius,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// </summary>
    /// <param name="mapper"><see cref="IPropertyMapper{TVirtualView,TViewHandler}"/>.</param>
    /// <param name="commandMapper"><see cref="CommandMapper"/>.</param>
    // ReSharper disable once UnusedParameter.Local
    public BottomSheetHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? BottomSheetMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// </summary>
    public BottomSheetHandler()
        : base(BottomSheetMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// </summary>
    /// <param name="context"><see cref="IMauiContext"/>.</param>
    public BottomSheetHandler(IMauiContext context)
        : base(BottomSheetMapper)
    {
        SetMauiContext(context);
    }
}
