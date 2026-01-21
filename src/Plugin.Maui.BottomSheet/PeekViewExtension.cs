namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Defines a XAML markup extension that provides the peek height value for a <see cref="BottomSheet"/> view.
/// </summary>
/// <remarks>
/// This extension allows setting the PeekHeight property of a <see cref="BottomSheet"/> by referencing another view,
/// providing better control over dynamic layout behavior within bottom sheets.
/// </remarks>
[ContentProperty(nameof(Name))]
[RequireService([typeof(IProvideValueTarget)])]
public sealed class PeekViewExtension : IMarkupExtension<double>
{
    private WeakReference<BottomSheet>? _bottomSheet;
    private WeakReference<View>? _view;

    /// <summary>
    /// Gets or sets the name of the view to be located within the BottomSheet's content.
    /// This property is used to find a specific child view within the BottomSheet
    /// using its name, enabling dynamic references and interactions.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the target View associated with the PeekView extension.
    /// This property specifies the reference to a View used to determine the
    /// height or content configuration for the Peek functionality in a BottomSheet.
    /// </summary>
    public View? View { get; set; }

    /// <summary>
    /// Provides a double value to the markup extension target object.
    /// This method sets up necessary event handlers for the target object,
    /// ensuring proper initialization and cleanup of resources tied to the BottomSheet instance.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider that provides access to services required by the markup extension, such as IProvideValueTarget.
    /// </param>
    /// <returns>
    /// A double value representing the initialized value provided to the target object.
    /// For this implementation, it always returns 0.
    /// </returns>
    public double ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget provideValueTarget = serviceProvider.GetRequiredService<IProvideValueTarget>();

        BottomSheet sheet = (BottomSheet)provideValueTarget.TargetObject;

        sheet.Loaded += SheetOnLoaded;
        sheet.Unloaded += SheetOnUnloaded;

        sheet.Opening += SheetOnOpening;
        sheet.Closing += SheetOnClosing;

        _bottomSheet = new(sheet);

        return 0;
    }

    /// <summary>
    /// Provides a value for the associated markup extension.
    /// </summary>
    /// <param name="serviceProvider">
    /// An object that provides services for the markup extension. This typically includes details
    /// about the target object and property where this extension is applied.
    /// </param>
    /// <returns>
    /// A <see cref="double"/> value that represents the provided result for this markup extension.
    /// </returns>
    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }

    /// <summary>
    /// Handles the Loaded event of the <see cref="BottomSheet"/> instance.
    /// Subscribes to the Opening and Closing events of the <see cref="BottomSheet"/>.
    /// </summary>
    /// <param name="sender">The source of the event, typically the <see cref="BottomSheet"/> instance.</param>
    /// <param name="e">An <see cref="EventArgs"/> that contains no event data.</param>
    private void SheetOnLoaded(object? sender, EventArgs e)
    {
        // Keep strong reference to the BottomSheet and View instances to prevent garbage collection.
        // Otherwise other events not fired. MAUI behavior.
    }

    /// <summary>
    /// Handles the Unloaded event for the BottomSheet. This method is responsible for detaching event handlers
    /// from the <see cref="BottomSheet"/> instance when it is unloaded, ensuring no memory leaks occur.
    /// </summary>
    /// <param name="sender">The source of the Unloaded event, typically the <see cref="BottomSheet"/> instance.</param>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    private void SheetOnUnloaded(object? sender, EventArgs e)
    {
        if (_bottomSheet?.TryGetTarget(out BottomSheet? sheet) == true)
        {
            sheet.Opening -= SheetOnOpening;
            sheet.Closing -= SheetOnClosing;
        }
    }

    /// <summary>
    /// Handles the event triggered when the <see cref="BottomSheet"/> is opening.
    /// This method performs initialization and subscribes to the <see cref="BottomSheet.LayoutChanged"/> event.
    /// </summary>
    /// <param name="sender">The source of the event, typically the <see cref="BottomSheet"/> instance.</param>
    /// <param name="e">The event data associated with the opening event.</param>
    private void SheetOnOpening(object? sender, EventArgs e)
    {
        if (_bottomSheet?.TryGetTarget(out BottomSheet? sheet) == true)
        {
            Init();

            sheet.LayoutChanged += SheetOnLayoutChanged;
        }
    }

    /// <summary>
    /// Handles the <see cref="BottomSheet.Closing"/> event which is triggered when the bottom sheet begins to close.
    /// This method unsubscribes from the <see cref="BottomSheet.LayoutChanged"/> event to clean up any associated event handlers.
    /// </summary>
    /// <param name="sender">The source of the event, typically the <see cref="BottomSheet"/> instance.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    private void SheetOnClosing(object? sender, EventArgs e)
    {
        _view = null;

        if (_bottomSheet?.TryGetTarget(out BottomSheet? sheet) == true)
        {
            sheet.LayoutChanged -= SheetOnLayoutChanged;
        }
    }

    /// <summary>
    /// Initializes the underlying <see cref="View"/> instance for the current <see cref="PeekViewExtension"/>.
    /// If the <see cref="View"/> is already associated, the method does nothing. Otherwise, it attempts to resolve
    /// the view based on the provided <see cref="Name"/> or directly assigns the <see cref="View"/> property if specified.
    /// </summary>
    /// <remarks>
    /// The <see cref="Name"/> property is used to search for a named view within the associated
    /// <see cref="BottomSheet"/> content. If the <see cref="Name"/> property is not set but the <see cref="View"/> property
    /// is specified, the provided <see cref="View"/> is directly associated. This method ensures that the resolved view
    /// is cached to optimize future access and minimize unnecessary calculations.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the <see cref="Name"/> is specified, but the associated view cannot be found in the
    /// <see cref="BottomSheet.Content"/> hierarchy.
    /// </exception>
    private void Init()
    {
        if (_view is null)
        {
            if (View is null)
            {
                if (string.IsNullOrWhiteSpace(Name) == false
                    && _bottomSheet?.TryGetTarget(out BottomSheet? bottomSheet) == true
                    && bottomSheet.Content?.Content is not null)
                {
                    _view = new(bottomSheet.Content.Content.FindByName<View>(Name));
                }
            }
            else
            {
                _view = new(View);
            }
        }
    }

    /// <summary>
    /// Handles the LayoutChanged event of the BottomSheet and adjusts the peek height
    /// of the BottomSheet dynamically based on its internal view hierarchy.
    /// </summary>
    /// <param name="sender">The source of the event, typically the BottomSheet instance.</param>
    /// <param name="e">An EventArgs object containing the event data.</param>
    private void SheetOnLayoutChanged(object? sender, EventArgs e)
    {
        SetPeekHeight();
    }

    /// <summary>
    /// Adjusts the peek height of the bottom sheet based on the measured heights
    /// of associated child views, such as the content, handle, and header rows.
    /// </summary>
    /// <remarks>
    /// This method calculates the total height of the views associated with the
    /// bottom sheet and assigns the calculated value to the PeekHeight property
    /// of the BottomSheet. It retrieves the heights of the main content view,
    /// handle view, and header view (if they exist) and sums them to determine
    /// the appropriate height for the bottom sheet in its minimized state.
    /// If no views are associated or cannot be measured, the PeekHeight will
    /// remain unaffected.
    /// </remarks>
    private void SetPeekHeight()
    {
        if (_bottomSheet?.TryGetTarget(out BottomSheet? bottomSheet) == true)
        {
            double peekHeight = 0;

            if (_view?.TryGetTarget(out View? view) == true)
            {
                peekHeight += view.Measure().Height;
            }

            if (bottomSheet.ContainerView.TryGetView(BottomSheet.HandleRow, out View? handle))
            {
                peekHeight += handle.Measure().Height;
            }

            if (bottomSheet.ContainerView.TryGetView(BottomSheet.HeaderRow, out View? header))
            {
                peekHeight += header.Measure().Height;
            }

            bottomSheet.PeekHeight = peekHeight;
        }
    }
}