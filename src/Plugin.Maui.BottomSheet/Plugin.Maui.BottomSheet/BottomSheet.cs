namespace Plugin.Maui.BottomSheet;

using System.Windows.Input;
using global::Maui.BindableProperty.Generator.Core;

/// <inheritdoc cref="IBottomSheet" />
public partial class BottomSheet : View, IBottomSheet
{
    private readonly WeakEventManager eventManager = new();

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="IBottomSheet"/> can be closed with gestures or manually.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private bool _isCancelable;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="IBottomSheet"/> can be closed with gestures or manually.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private bool _hasHandle;

    /// <summary>
    /// Gets or sets a value indicating whether showing the <see cref="BottomSheetHeader"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private bool _showHeader;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="IBottomSheet"/>. is open.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private bool _isOpen;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="IBottomSheet"/> can be closed with gestures or manually.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private bool _isDraggable;

    /// <summary>
    /// Gets or sets the <see cref="BottomSheetHeader"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private BottomSheetHeader? _header;

    /// <summary>
    /// Gets or sets allowed <see cref="IBottomSheet"/> states.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private ICollection<BottomSheetState>? _states;

    /// <summary>
    /// Gets or sets current <see cref="IBottomSheet"/> state.
    /// </summary>
    /// <remarks><see cref="BottomSheetState.None"/> if state is unknown.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private BottomSheetState _currentState;

    /// <summary>
    /// Gets or sets the <see cref="BottomSheetPeek"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private BottomSheetPeek? _peek;

    /// <summary>
    /// Gets or sets the executed command when the <see cref="IBottomSheet"/> is closing.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private ICommand? _closingCommand;

    /// <summary>
    /// Gets or sets the <see cref="BottomSheet.ClosingCommand"/> parameter.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private object? _closingCommandParameter;

    /// <summary>
    /// Gets or sets the executed command when the <see cref="IBottomSheet"/> is closed.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private ICommand? _closedCommand;

    /// <summary>
    /// Gets or sets the <see cref="BottomSheet.ClosedCommand"/> parameter.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private object? _closedCommandParameter;

    /// <summary>
    /// Gets or sets the executed command when the <see cref="IBottomSheet"/> is opening.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private ICommand? _openingCommand;

    /// <summary>
    /// Gets or sets the <see cref="BottomSheet.OpeningCommand"/> parameter.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private object? _openingCommandParameter;

    /// <summary>
    /// Gets or sets the executed command when the <see cref="IBottomSheet"/> is opened.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private ICommand? _openedCommand;

    /// <summary>
    /// Gets or sets the <see cref="BottomSheet.OpenedCommand"/> parameter.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private object? _openedCommandParameter;

    /// <summary>
    /// Gets or sets the content <see cref="DataTemplate"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private object? _contentTemplate;

    /// <inheritdoc/>
    public event EventHandler Closing
    {
        add => eventManager.AddEventHandler(value);
        remove => eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc/>
    public event EventHandler Closed
    {
        add => eventManager.AddEventHandler(value);
        remove => eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc/>
    public event EventHandler Opening
    {
        add => eventManager.AddEventHandler(value);
        remove => eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc/>
    public event EventHandler Opened
    {
        add => eventManager.AddEventHandler(value);
        remove => eventManager.RemoveEventHandler(value);
    }
}