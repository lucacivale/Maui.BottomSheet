namespace Plugin.Maui.BottomSheet.Handlers;

using Plugin.Maui.BottomSheet;

/// <summary>
/// Provides functionality for managing BottomSheet components in the MAUI framework across multiple platforms.
/// </summary>
/// <remarks>
/// The <see cref="BottomSheetHandler"/> integrates platform-specific views with the <see cref="IBottomSheet"/> interface,
/// handling property mapping, commands, and lifecycle management for bottom sheet components.
/// </remarks>
public sealed partial class BottomSheetHandler
{
    /// <summary>
    /// Defines a static property mapper for the <see cref="IBottomSheet"/> interface,
    /// associating its properties with corresponding mapping methods in the <see cref="BottomSheetHandler"/> class.
    /// </summary>
    /// <remarks>
    /// This property mapper enables synchronization between the properties of an <see cref="IBottomSheet"/>
    /// instance and the logic implemented within the <see cref="BottomSheetHandler"/>. It ensures that updates
    /// to the <see cref="IBottomSheet"/> properties are appropriately handled and reflected in its behavior.
    /// </remarks>
    private static readonly IPropertyMapper<IBottomSheet, BottomSheetHandler> _bottomSheetMapper =
        new PropertyMapper<IBottomSheet, BottomSheetHandler>(ElementMapper)
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

    /// <summary>
    /// Represents a command mapper for the <see cref="IBottomSheet"/> interface,
    /// mapping its commands to corresponding handler methods in the <see cref="BottomSheetHandler"/> class.
    /// </summary>
    /// <remarks>
    /// This static command mapper associates the commands defined in the <see cref="IBottomSheet"/> interface
    /// with specific handling logic in the <see cref="BottomSheetHandler"/> implementation. It ensures that
    /// commands like <c>Cancel</c> and platform-specific behaviors, such as setting margins or half-expanded
    /// ratios on Android, are correctly mapped to their respective methods for execution.
    /// </remarks>
    private static readonly CommandMapper<IBottomSheet, BottomSheetHandler> _bottomSheetCommandMapper = new(ElementCommandMapper)
    {
        [nameof(IBottomSheet.Cancel)] = MapCancel,
#if ANDROID
        [nameof(PlatformConfiguration.AndroidSpecific.BottomSheet.SetMargin)] = MapMargin,
        [nameof(PlatformConfiguration.AndroidSpecific.BottomSheet.SetHalfExpandedRatio)] = MapHalfExpandedRatio,
#endif
#if WINDOWS
        [nameof(PlatformConfiguration.WindowsSpecific.BottomSheet.SetMaxWidth)] = MapMaxWidth,
        [nameof(PlatformConfiguration.WindowsSpecific.BottomSheet.SetMaxHeight)] = MapMaxHeight,
        [nameof(PlatformConfiguration.WindowsSpecific.BottomSheet.SetMinWidth)] = MapMinWidth,
        [nameof(PlatformConfiguration.WindowsSpecific.BottomSheet.SetMinHeight)] = MapMinHeight,
#endif
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// Represents a handler for configuring and managing bottom sheet behavior.
    /// This class bridges the behavior and properties of the virtual bottom sheet view
    /// with the platform-specific native implementation.
    /// </summary>
    /// <param name="mapper">The property mapper for the bottom sheet view.</param>
    /// <param name="commandMapper">The command mapper for the bottom sheet view.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "S1118:Utility classes should not have public constructors", Justification = "Must be public.")]
    public BottomSheetHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? _bottomSheetMapper, commandMapper ?? _bottomSheetCommandMapper)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandler"/> class.
    /// Represents a handler for managing and mapping properties and commands
    /// of the BottomSheet component between the .NET MAUI core abstraction
    /// and platform-specific implementations.
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
    /// <param name="context">The <see cref="IMauiContext"/> instance associated with the handler.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "S1118:Utility classes should not have public constructors", Justification = "Must be public.")]
    public BottomSheetHandler(IMauiContext context)
        : base(_bottomSheetMapper, _bottomSheetCommandMapper)
    {
        SetMauiContext(context);
    }

    /// <summary>
    /// Gets a value indicating whether the handler is currently in the process of establishing
    /// a connection between the virtual view and the platform-specific view.
    /// </summary>
    /// <remarks>
    /// This property is primarily used internally to track the state of the
    /// view-binding process. It is set to <c>true</c> when the virtual view is being
    /// assigned to the handler and set back to <c>false</c> once the process completes.
    /// </remarks>
    public bool IsConnecting { get; private set; }

    /// <summary>
    /// Sets the virtual view for this handler. This method associates the specified virtual view
    /// with the current handler and applies the necessary configurations to synchronize the view
    /// with the platform-specific implementation.
    /// </summary>
    /// <param name="view">The virtual view to be assigned to this handler.</param>
    public override void SetVirtualView(IView view)
    {
        IsConnecting = true;
        base.SetVirtualView(view);
        IsConnecting = false;
    }

    /// <summary>
    /// Opens the bottom sheet asynchronously. This method initiates the process
    /// of displaying the bottom sheet associated with the handler.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of opening the bottom sheet.</returns>
    internal partial Task OpenAsync();

    /// <summary>
    /// Closes the currently displayed bottom sheet asynchronously, ensuring that any associated resources or handlers
    /// are properly released. This method helps manage the lifecycle and dismisses the bottom sheet in a platform-specific manner.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of closing the bottom sheet.</returns>
    internal partial Task CloseAsync();
}
