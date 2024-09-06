namespace Plugin.Maui.BottomSheet.Handlers;

using Plugin.Maui.BottomSheet;

/// <summary>
/// <see cref="IBottomSheet"/> handler.
/// </summary>
public sealed partial class BottomSheetHandler
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
        [nameof(IBottomSheet.ContentTemplate)] = MapContentTemplate,
    };

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
