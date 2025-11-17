namespace Plugin.Maui.BottomSheet.Handlers;

using Plugin.Maui.BottomSheet;

/// <summary>
/// Represents a handler for managing BottomSheet components within the Maui framework.
/// </summary>
/// <remarks>
/// The <see cref="BottomSheetHandler"/> provides mapping and handling of various properties and commands
/// for the <see cref="IBottomSheet"/> interface in an application, and facilitates its integration with
/// the underlying platform-specific implementation.
/// </remarks>
internal sealed partial class BottomSheetHandler
{
    /// <summary>
    /// Represents a property mapper for the <see cref="IBottomSheet"/> interface,
    /// mapping its properties to corresponding handler methods in the <see cref="BottomSheetHandler"/>.
    /// </summary>
    /// <remarks>
    /// This static property mapper is responsible for binding the properties of an <see cref="IBottomSheet"/> instance
    /// to the appropriate functionality within the <see cref="BottomSheetHandler"/>. The mapping ensures
    /// that any changes to the properties of the <see cref="IBottomSheet"/> are reflected in the handler logic.
    /// </remarks>
    // ReSharper disable once ArrangeModifiersOrder
    private static readonly IPropertyMapper<IBottomSheet, BottomSheetHandler> _bottomSheetMapper = new PropertyMapper<IBottomSheet, BottomSheetHandler>(ElementMapper)
    {
        [nameof(IBottomSheet.IsCancelable)] = MapIsCancelable,
        [nameof(IBottomSheet.IsOpen)] = MapIsOpen,
        [nameof(IBottomSheet.IsDraggable)] = MapIsDraggable,
        [nameof(IBottomSheet.States)] = MapStates,
        [nameof(IBottomSheet.CurrentState)] = MapCurrentState,
        [nameof(IBottomSheet.PeekHeight)] = MapPeekHeight,
        [nameof(IBottomSheet.BackgroundColor)] = MapBackgroundColor,
        [nameof(IBottomSheet.CornerRadius)] = MapCornerRadius,
        [nameof(IBottomSheet.WindowBackgroundColor)] = MapWindowBackgroundColor,
        [nameof(IBottomSheet.IsModal)] = MapIsModal,
    };

    private static readonly CommandMapper<IBottomSheet, BottomSheetHandler> _bottomSheetCommandMapper = new CommandMapper<IBottomSheet, BottomSheetHandler>(ElementCommandMapper)
    {
        [nameof(IBottomSheet.Cancel)] = MapCancel,
        [nameof(PlatformConfiguration.AndroidSpecific.BottomSheet.SetMargin)] = MapMargin,
        [nameof(PlatformConfiguration.AndroidSpecific.BottomSheet.SetHalfExpandedRatio)] = MapHalfExpandedRatio,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// Handles bottom sheet behavior and maps properties between a virtual view and a native view implementation.
    /// </summary>
    /// <param name="mapper">Property mapper.</param>
    /// <param name="commandMapper">Command mapper.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "S1118:Utility classes should not have public constructors", Justification = "Must be public.")]
    public BottomSheetHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? _bottomSheetMapper, commandMapper ?? _bottomSheetCommandMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// Handles the lifecycle and behavior of a BottomSheet component within a .NET MAUI application.
    /// Implements behavior mappings between the BottomSheet interface and its platform-specific representation.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "S1118:Utility classes should not have public constructors", Justification = "Must be public.")]
    public BottomSheetHandler()
        : base(_bottomSheetMapper, _bottomSheetCommandMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// Represents the handler responsible for managing the behavior and presentation
    /// of a bottom sheet within a .NET MAUI application.
    /// </summary>
    /// <param name="context">Maui context.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "S1118:Utility classes should not have public constructors", Justification = "Must be public.")]
    public BottomSheetHandler(IMauiContext context)
        : base(_bottomSheetMapper, _bottomSheetCommandMapper)
    {
        SetMauiContext(context);
    }

    /// <summary>
    /// Gets a value indicating whether the connection process is currently underway.
    /// </summary>
    /// <remarks>
    /// This property reflects the state of connectivity during specific
    /// operations, such as when setting the virtual view. It is set to
    /// true at the beginning of the connection process and to false
    /// once the process is complete.
    /// </remarks>
    public bool IsConnecting { get; private set; }

    /// <summary>
    /// Sets the virtual view for this handler. This attaches the given view
    /// to the handler and updates the required configurations.
    /// </summary>
    /// <param name="view">The virtual view to be set for this handler.</param>
    public override void SetVirtualView(IView view)
    {
        IsConnecting = true;
        base.SetVirtualView(view);
        IsConnecting = false;
    }

    /// <summary>
    /// Opens the bottom sheet asynchronously. This method initiates the opening process
    /// for the current bottom sheet associated with the handler.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of opening the bottom sheet.</returns>
    internal partial Task OpenAsync();

    /// <summary>
    /// Closes the currently open bottom sheet asynchronously and releases any associated resources or handlers.
    /// This method ensures that the bottom sheet is properly closed, its handler is disconnected,
    /// and navigation events are triggered as necessary when the sheet is dismissed.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of closing the bottom sheet.</returns>
    internal partial Task CloseAsync();
}
