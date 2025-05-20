namespace Plugin.Maui.BottomSheet.Handlers;

using Plugin.Maui.BottomSheet;

/// <summary>
/// <see cref="IBottomSheet"/> handler.
/// </summary>
internal sealed partial class BottomSheetHandler
{
    // ReSharper disable once ArrangeModifiersOrder
    private static readonly IPropertyMapper<IBottomSheet, BottomSheetHandler> _bottomSheetMapper = new PropertyMapper<IBottomSheet, BottomSheetHandler>(ElementMapper)
    {
        [nameof(IBottomSheet.IsCancelable)] = MapIsCancelable,
        [nameof(IBottomSheet.HasHandle)] = MapHasHandle,
        [nameof(IBottomSheet.ShowHeader)] = MapShowHeader,
        [nameof(IBottomSheet.IsOpen)] = MapIsOpen,
        [nameof(IBottomSheet.IsDraggable)] = MapIsDraggable,
        [nameof(IBottomSheet.Header)] = MapHeader,
        [nameof(IBottomSheet.States)] = MapStates,
        [nameof(IBottomSheet.CurrentState)] = MapCurrentState,
        [nameof(IBottomSheet.PeekHeight)] = MapPeekHeight,
        [nameof(IBottomSheet.Content)] = MapContent,
        [nameof(IBottomSheet.Padding)] = MapPadding,
        [nameof(IBottomSheet.BackgroundColor)] = MapBackgroundColor,
        [nameof(IBottomSheet.IgnoreSafeArea)] = MapIgnoreSafeArea,
        [nameof(IBottomSheet.CornerRadius)] = MapCornerRadius,
        [nameof(IBottomSheet.WindowBackgroundColor)] = MapWindowBackgroundColor,
        [nameof(IBottomSheet.IsModal)] = MapIsModal,
        [nameof(IBottomSheet.BottomSheetStyle)] = MapBottomSheetStyle,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// </summary>
    /// <param name="mapper"><see cref="IPropertyMapper{TVirtualView,TViewHandler}"/>.</param>
    /// <param name="commandMapper"><see cref="CommandMapper"/>.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "S1118:Utility classes should not have public constructors", Justification = "Must be public.")]
    public BottomSheetHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? _bottomSheetMapper, commandMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "S1118:Utility classes should not have public constructors", Justification = "Must be public.")]
    public BottomSheetHandler()
        : base(_bottomSheetMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// </summary>
    /// <param name="context"><see cref="IMauiContext"/>.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "S1118:Utility classes should not have public constructors", Justification = "Must be public.")]
    public BottomSheetHandler(IMauiContext context)
        : base(_bottomSheetMapper)
    {
        SetMauiContext(context);
    }

    /// <summary>
    /// Gets a value indicating whether handler connecting.
    /// </summary>
    public bool IsConnecting { get; private set; }

    public override void SetVirtualView(IView view)
    {
        IsConnecting = true;
        base.SetVirtualView(view);
        IsConnecting = false;
    }

    /// <summary>
    /// Open bottom sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    internal partial Task OpenAsync();

    /// <summary>
    /// Close bottom sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    internal partial Task CloseAsync();
}
