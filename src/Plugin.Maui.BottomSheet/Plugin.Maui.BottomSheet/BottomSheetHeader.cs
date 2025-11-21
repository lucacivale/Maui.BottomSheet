using System.Diagnostics.CodeAnalysis;
using Plugin.BottomSheet;

namespace Plugin.Maui.BottomSheet;

using Microsoft.Maui.Controls;

/// <summary>
/// Defines the header component for a bottom sheet, providing a customizable area for displaying a title, indicators, or controls.
/// </summary>
public sealed class BottomSheetHeader : BottomSheetContentView, IBottomSheetHeader
{
    /// <summary>
    /// Represents a bindable property for the title text value.
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(
            nameof(TitleText),
            typeof(string),
            typeof(BottomSheetHeader),
            propertyChanged: OnTitleTextPropertyChanged);

    /// <summary>
    /// Bindable property for the top-left button.
    /// </summary>
    public static readonly BindableProperty TopLeftButtonProperty =
        BindableProperty.Create(
            nameof(TopLeftButton),
            typeof(Button),
            typeof(BottomSheetHeader),
            propertyChanged: OnTopLeftButtonPropertyChanged);

    /// <summary>
    /// Bindable property for the top-right button.
    /// </summary>
    public static readonly BindableProperty TopRightButtonProperty =
        BindableProperty.Create(
            nameof(TopRightButton),
            typeof(Button),
            typeof(BottomSheetHeader),
            propertyChanged: OnTopRightButtonPropertyChanged);

    /// <summary>
    /// Bindable property for controlling the visibility of the close button.
    /// </summary>
    public static readonly BindableProperty ShowCloseButtonProperty =
        BindableProperty.Create(
            nameof(ShowCloseButton),
            typeof(bool),
            typeof(BottomSheetHeader),
            propertyChanged: OnShowCloseButtonPropertyChanged);

    /// <summary>
    /// Bindable property for the position of the close button.
    /// </summary>
    public static readonly BindableProperty CloseButtonPositionProperty =
        BindableProperty.Create(
            nameof(BottomSheetHeaderCloseButtonPosition),
            typeof(BottomSheetHeaderCloseButtonPosition),
            typeof(BottomSheetHeader),
            defaultValue: BottomSheetHeaderCloseButtonPosition.TopRight,
            propertyChanged: OnCloseButtonPositionPropertyChanged);

    /// <summary>
    /// Bindable property for the appearance of the header.
    /// </summary>
    public static readonly BindableProperty HeaderAppearanceProperty =
        BindableProperty.Create(
            nameof(HeaderAppearance),
            typeof(BottomSheetHeaderButtonAppearanceMode),
            typeof(BottomSheetHeader),
            propertyChanged: OnHeaderAppearancePropertyChanged);

    /// <summary>
    /// Bindable property for defining and customizing the visual style.
    /// </summary>
    public static readonly BindableProperty StyleProperty =
        BindableProperty.Create(
            nameof(Style),
            typeof(BottomSheetHeaderStyle),
            typeof(BottomSheetHeader),
            propertyChanged: OnStyleChanged);

    private const int TopLeftButtonColumn = 0;
    private const int TitleColumn = 1;
    private const int TopRightButtonColumn = 2;

    private readonly WeakEventManager _weakEventManager = new();

    private Grid? _builtInHeaderLayout;

    /// <summary>
    /// Event triggered when the close button is clicked.
    /// </summary>
    public event EventHandler CloseButtonClicked
    {
        add => _weakEventManager.AddEventHandler(value);
        remove => _weakEventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets the text displayed as the title.
    /// </summary>
    public string? TitleText
    {
        get => (string?)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the top-left button.
    /// </summary>
    public Button? TopLeftButton
    {
        get => (Button?)GetValue(TopLeftButtonProperty);
        set => SetValue(TopLeftButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the button located at the top-right position.
    /// </summary>
    public Button? TopRightButton
    {
        get => (Button?)GetValue(TopRightButtonProperty);
        set => SetValue(TopRightButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the position of the close button.
    /// </summary>
    public BottomSheetHeaderCloseButtonPosition CloseButtonPosition
    {
        get => (BottomSheetHeaderCloseButtonPosition)GetValue(CloseButtonPositionProperty);
        set => SetValue(CloseButtonPositionProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the close button is visible.
    /// </summary>
    public bool ShowCloseButton
    {
        get => (bool)GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual appearance and formatting rules.
    /// </summary>
    public BottomSheetHeaderStyle Style
    {
        get => (BottomSheetHeaderStyle)GetValue(StyleProperty);
        set => SetValue(StyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the appearance of the header.
    /// </summary>
    public BottomSheetHeaderButtonAppearanceMode HeaderAppearance
    {
        get => (BottomSheetHeaderButtonAppearanceMode)GetValue(HeaderAppearanceProperty);
        set => SetValue(HeaderAppearanceProperty, value);
    }

    /// <summary>
    /// Gets the top-left button.
    /// </summary>
    object? IBottomSheetHeader.TopLeftButton => TopLeftButton;

    /// <summary>
    /// Gets the button located in the top-right corner.
    /// </summary>
    object? IBottomSheetHeader.TopRightButton => TopRightButton;

    /// <summary>
    /// Gets the content.
    /// </summary>
    object? IBottomSheetContentView.Content => Content;

    /// <summary>
    /// Gets the content template.
    /// </summary>
    object? IBottomSheetContentView.ContentTemplate => ContentTemplate;

    /// <summary>
    /// Creates and returns the content for the associated component or object.
    /// </summary>
    /// <returns>The created content as an object, ready for use or rendering.
    /// </returns>
    internal override View CreateContent()
    {
        View view;

        if (this.HasHeaderView())
        {
            view = base.CreateContent();
        }
        else
        {
            _builtInHeaderLayout = new Grid
            {
                RowDefinitions = new RowDefinitionCollection(new RowDefinition(GridLength.Star)),
                ColumnDefinitions = new ColumnDefinitionCollection(
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(new GridLength(2, GridUnitType.Star)),
                    new ColumnDefinition(GridLength.Star)),
                ColumnSpacing = 5,
                AutomationId = AutomationIds.Header,
            };

            if (this.HasTopLeftButton())
            {
                _ = TryAdd(TopLeftButton, TopLeftButtonColumn);
            }

            if (this.HasTitle())
            {
                _ = TryAddTitle();
            }

            if (this.HasTopRightButton())
            {
                _ = TryAdd(TopRightButton, TopRightButtonColumn);
            }

            if (this.HasTopLeftCloseButton()
                || this.HasTopRightCloseButton())
            {
                _ = TryAdd(CreateCloseButton(), CloseButtonColumn());
            }

            view = _builtInHeaderLayout;
        }

        return view;
    }

    /// <summary>
    /// Removes the specified element or elements from the collection or data structure.
    /// </summary>
    internal override void Remove()
    {
        base.Remove();

        if (_builtInHeaderLayout is not null)
        {
            _builtInHeaderLayout.Children.Clear();
            _builtInHeaderLayout.Parent = null;
            _builtInHeaderLayout.BindingContext = null;
            _builtInHeaderLayout.DisconnectHandlers();
            _builtInHeaderLayout = null;
        }
    }

    /// <summary>
    /// Invoked when the parent of the current element changes.
    /// </summary>
    /// <param name="parent">The new parent element.</param>
    protected override void OnParentChanged(Element parent)
    {
        base.OnParentChanged(parent);

        if (_builtInHeaderLayout is not null)
        {
            _builtInHeaderLayout.Parent = parent;
        }
    }

    /// <summary>
    /// Called when the binding context of the element changes.
    /// </summary>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        OnBindingContextChanged(_builtInHeaderLayout);
    }

    /// <summary>
    /// Handles the property changed event for the <c>TopLeftButton</c> bindable property.
    /// Updates the internal state of the <see cref="BottomSheetHeader"/> component when the
    /// <c>TopLeftButton</c> property value is modified.
    /// </summary>
    /// <param name="bindable">The bindable object containing the <c>TopLeftButton</c> property.</param>
    /// <param name="oldValue">The previous value of the <c>TopLeftButton</c> property.</param>
    /// <param name="newValue">The new value of the <c>TopLeftButton</c> property.</param>
    private static void OnTopLeftButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnTopLeftButtonPropertyChanged((Button)newValue);

    /// <summary>
    /// Called when the <see cref="TopRightButtonProperty"/> changes.
    /// Updates the state or behavior of the <see cref="BottomSheetHeader"/> based on the newly assigned <see cref="Button"/>.
    /// </summary>
    /// <param name="bindable">The object on which the property changed, typically the <see cref="BottomSheetHeader"/> instance.</param>
    /// <param name="oldValue">The previous value of the <see cref="TopRightButtonProperty"/>.</param>
    /// <param name="newValue">The new value of the <see cref="TopRightButtonProperty"/>.</param>
    private static void OnTopRightButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnTopRightButtonPropertyChanged((Button)newValue);

    /// <summary>
    /// Callback invoked when the value of the <see cref="ShowCloseButtonProperty"/> changes.
    /// Performs actions to reflect the updated state of the ShowCloseButton property.
    /// </summary>
    /// <param name="bindable">The object on which the property has changed.</param>
    /// <param name="oldValue">The old value of the property before the change occurred.</param>
    /// <param name="newValue">The new value of the property after the change occurred.</param>
    private static void OnShowCloseButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnShowCloseButtonPropertyChanged();

    /// <summary>
    /// Handles changes to the CloseButtonPositionProperty of a <see cref="BottomSheetHeader"/>.
    /// This property determines the position of the close button in the header, such as TopLeft or TopRight.
    /// </summary>
    /// <param name="bindable">
    /// The object to which the property is attached. This is expected to be an instance of <see cref="BottomSheetHeader"/>.
    /// </param>
    /// <param name="oldValue">
    /// The previous value of the property, represented as an object of type <see cref="BottomSheetHeaderCloseButtonPosition"/>.
    /// </param>
    /// <param name="newValue">
    /// The new value of the property, represented as an object of type <see cref="BottomSheetHeaderCloseButtonPosition"/>.
    /// </param>
    private static void OnCloseButtonPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnCloseButtonPositionPropertyChanged((BottomSheetHeaderCloseButtonPosition)oldValue);

    /// <summary>
    /// Handles the logic to execute when the value of the <see cref="HeaderAppearanceProperty"/> changes.
    /// This method is triggered automatically when the property is updated on the <see cref="BottomSheetHeader"/> instance.
    /// </summary>
    /// <param name="bindable">
    /// The bindable object that holds the <see cref="HeaderAppearanceProperty"/>.
    /// Typically, an instance of <see cref="BottomSheetHeader"/>.
    /// </param>
    /// <param name="oldValue">
    /// The old value of the <see cref="HeaderAppearanceProperty"/> before the change.
    /// </param>
    /// <param name="newValue">
    /// The new value of the <see cref="HeaderAppearanceProperty"/> after the change.
    /// </param>
    private static void OnHeaderAppearancePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnHeaderAppearancePropertyChanged();

    /// <summary>
    /// Invoked when the value of the <c>TitleText</c> property changes.
    /// </summary>
    /// <param name="bindable">The object that contains the property that changed.</param>
    /// <param name="oldValue">The previous value of the <c>TitleText</c> property.</param>
    /// <param name="newValue">The new value of the <c>TitleText</c> property.</param>
    private static void OnTitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnTitleTextPropertyChanged((string)oldValue);

    /// <summary>
    /// Called when the Style property of the BottomSheetHeader changes.
    /// This method allows for any updates or adjustments in response to the style change.
    /// </summary>
    /// <param name="bindable">The bindable object where the property change occurred.</param>
    /// <param name="oldValue">The previous value of the Style property.</param>
    /// <param name="newValue">The new value of the Style property.</param>
    private static void OnStyleChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnStyleChanged();

    /// <summary>
    /// Invoked when the value of the <c>Style</c> property changes.
    /// </summary>
    /// <param name="bindable">The object that contains the property.</param>
    /// <param name="oldValue">The previous value of the <c>Style</c> property.</param>
    /// <param name="newValue">The new value of the <c>Style</c> property.</param>
    private void OnStyleChanged()
    {
        SetTitleStyle();
        SetCloseButtonStyle();
    }

    /// <summary>
    /// Configures and applies the visual style settings for the title element
    /// in the BottomSheetHeader, including properties such as text color,
    /// font size, font attributes, font family, and font auto-scaling.
    /// </summary>
    /// <remarks>
    /// This method binds the title element's styling properties to the corresponding
    /// properties in the BottomSheetHeaderStyle instance. It ensures the title
    /// reflects the desired appearance as defined in the style.
    /// </remarks>
    private void SetTitleStyle()
    {
        if (this.HasTitle()
            && TryGetView(TitleColumn, out Label? title))
        {
            title.SetBinding(Label.TextColorProperty, static (BottomSheetHeaderStyle style) => style.TitleTextColor, source: Style);
            title.SetBinding(Label.FontSizeProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontSize, source: Style);
            title.SetBinding(Label.FontAttributesProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontAttributes, source: Style);
            title.SetBinding(Label.FontFamilyProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontFamily, source: Style);
            title.SetBinding(Label.FontAutoScalingEnabledProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontAutoScalingEnabled, source: Style);
        }
    }

    /// <summary>
    /// Applies the appropriate style to the close button, ensuring it adheres to
    /// the design guidelines and maintains a consistent appearance.
    /// </summary>
    private void SetCloseButtonStyle()
    {
        if ((this.HasTopLeftCloseButton()
                || this.HasTopRightCloseButton())
            && TryGetView(CloseButtonColumn(), out CloseButton? closeButton))
        {
            closeButton.SetBinding(CloseButton.TintProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonTintColor, source: Style);
            closeButton.SetBinding(VisualElement.HeightRequestProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonHeightRequest, source: Style);
            closeButton.SetBinding(VisualElement.WidthRequestProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonWidthRequest, source: Style);
        }
    }

    /// <summary>
    /// Handles changes to the TitleText property of the <see cref="BottomSheetHeader"/>.
    /// This method ensures that the title column is displayed or removed based on whether
    /// the TitleText value is set or cleared.
    /// </summary>
    /// <param name="oldValue">
    /// The previous value of the TitleText property before the change.
    /// </param>
    private void OnTitleTextPropertyChanged(string oldValue)
    {
        if (this.HasTitle() == false)
        {
            _ = TryRemove(TitleColumn);
        }
        else if (string.IsNullOrWhiteSpace(oldValue))
        {
            _ = TryAddTitle();
        }
    }

    /// <summary>
    /// Handles changes to the <see cref="BottomSheetHeader.TopLeftButtonProperty"/>.
    /// Updates the visual representation of the top-left button in the header
    /// when the property value is modified.
    /// </summary>
    /// <param name="newButton">
    /// The new <see cref="Button"/> instance that replaces the previous top-left button.
    /// </param>
    private void OnTopLeftButtonPropertyChanged(Button newButton)
    {
        OnTopButtonChanged(newButton, TopLeftButtonColumn, this.HasTopLeftButton());
    }

    /// <summary>
    /// Invoked when the value of the <c>TopRightButton</c> property changes.
    /// </summary>
    /// <param name="newButton">The new value assigned to the <c>TopRightButton</c> property.</param>
    private void OnTopRightButtonPropertyChanged(Button newButton)
    {
        OnTopButtonChanged(newButton, TopRightButtonColumn, this.HasTopRightButton());
    }

    /// <summary>
    /// Invoked when the value of the <c>TopButton</c> property changes.
    /// </summary>
    /// <param name="newButton">The new value assigned to the <c>TopButton</c> property.</param>
    /// <param name="column">The column index where the button is located.</param>
    /// <param name="addNewButton">Indicates whether the button should be added to the layout.</param>
    private void OnTopButtonChanged(Button newButton, int column, bool addNewButton)
    {
        _ = TryRemove(column);

        if (addNewButton)
        {
            _ = TryAdd(newButton, column);
        }
    }

    /// <summary>
    /// Responds to changes in the value of the <see cref="ShowCloseButtonProperty"/>.
    /// Invoked automatically when the <see cref="ShowCloseButton"/> property value changes.
    /// </summary>
    /// <remarks>
    /// This method arranges the close button in the layout based on the defined button position.
    /// It ensures the close button is displayed or removed correctly depending on the updated property value.
    /// </remarks>
    private void OnShowCloseButtonPropertyChanged()
    {
        ArrangeCloseButton(CloseButtonColumn());
    }

    /// <summary>
    /// Handles changes to the <see cref="CloseButtonPositionProperty"/> by re-arranging
    /// the close button's position in the header layout based on its new value.
    /// </summary>
    /// <param name="oldValue">
    /// The previous value of <see cref="BottomSheetHeaderCloseButtonPosition"/> before the property was updated.
    /// </param>
    private void OnCloseButtonPositionPropertyChanged(BottomSheetHeaderCloseButtonPosition oldValue)
    {
        int column = oldValue == BottomSheetHeaderCloseButtonPosition.TopLeft ? TopLeftButtonColumn : TopRightButtonColumn;

        ArrangeCloseButton(column);
    }

    /// <summary>
    /// Invoked when the value of the <c>HeaderAppearance</c> property changes.
    /// </summary>
    /// <param name="bindable">The object that includes the property being monitored.</param>
    /// <param name="oldValue">The previous value of the <c>HeaderAppearance</c> property.</param>
    /// <param name="newValue">The new value assigned to the <c>HeaderAppearance</c> property.</param>
    private void OnHeaderAppearancePropertyChanged()
    {
        if (this.HasTopLeftButton() == false
            && this.HasTopLeftCloseButton() == false)
        {
            _ = TryRemove(TopLeftButtonColumn);
        }
        else if (TryGetView(TopLeftButtonColumn, out View? _) == false)
        {
            View view = this.HasTopLeftButton() ? TopLeftButton! : CreateCloseButton();
            _ = TryAdd(view, TopLeftButtonColumn);
        }

        if (this.HasTopRightButton() == false
            && this.HasTopRightCloseButton() == false)
        {
            _ = TryRemove(TopRightButtonColumn);
        }
        else if (TryGetView(TopRightButtonColumn, out View? _) == false)
        {
            View view = this.HasTopRightButton() ? TopRightButton! : CreateCloseButton();
            _ = TryAdd(view, TopRightButtonColumn);
        }
    }

    /// <summary>
    /// Attempts to retrieve the layout associated with the specified key.
    /// </summary>
    /// <param name="grid">When this method returns, contains the layout associated with the specified key if found; otherwise, null.</param>
    /// <returns><c>true</c> if the layout exists for the specified key; otherwise, <c>false</c>.</returns>
    private bool TryGetLayout([NotNullWhen(true)] out Grid? grid)
    {
        bool ret = false;
        grid = null;

        if (_builtInHeaderLayout is Grid gridContent)
        {
            grid = gridContent;
            ret = true;
        }

        return ret;
    }

    /// <summary>
    /// Attempts to retrieve a view for the specified key.
    /// </summary>
    /// <param name="column">The column index of the view to retrieve.</param>
    /// <param name="view">When this method returns, contains the view associated with the specified key if found; otherwise, null.</param>
    /// <typeparam name="TView">The type of the view to retrieve.</typeparam>
    /// <returns>True if a view with the specified key is found; otherwise, false.</returns>
    private bool TryGetView<TView>(int column, [NotNullWhen(true)] out TView? view)
        where TView : View
    {
        bool ret = false;
        view = null;

        if (TryGetLayout(out Grid? grid)
            && grid.Children.FirstOrDefault(child => grid.GetColumn(child) == column) is TView gridView)
        {
            view = gridView;
            ret = true;
        }

        return ret;
    }

    /// <summary>
    /// Attempts to remove the element with the specified key from the collection.
    /// </summary>
    /// <param name="column">The column index of the element to remove.</param>
    /// <returns>True if the element is successfully found and removed; otherwise, false.</returns>
    private bool TryRemove(int column)
    {
        if (TryGetLayout(out Grid? grid) == false)
        {
            return false;
        }

        if (TryGetView(column, out View? view))
        {
            grid.Remove(view);

            view.BindingContext = null;
            view.DisconnectHandlers();

            if (view is CloseButton closeButton)
            {
                closeButton.Clicked -= CloseButtonOnClicked;
            }
        }

        return true;
    }

    /// <summary>
    /// Attempts to add a specified key and value to the collection.
    /// </summary>
    /// <param name="view">The view to be added to the collection.</param>
    /// <param name="column">The column index where the view should be added.</param>
    /// <returns>
    /// <c>true</c> if the key and value pair was successfully added;
    /// otherwise, <c>false</c> if the key already exists in the collection.
    /// </returns>
    private bool TryAdd(View? view, int column)
    {
        if (TryGetLayout(out Grid? grid) == false
            || view is null)
        {
            return false;
        }

        _ = TryRemove(column);

        view.BindingContext = BindingContext;
        grid.Add(view, column);

        if (column == TitleColumn)
        {
            SetTitleStyle();
        }
        else if (column == CloseButtonColumn())
        {
            SetCloseButtonStyle();
        }

        return true;
    }

    /// <summary>
    /// Attempts to add a title to the collection if it does not already exist.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the title was successfully added; otherwise, <c>false</c>.
    /// </returns>
    private bool TryAddTitle()
    {
        if (TryGetView(TitleColumn, out View? _))
        {
            return false;
        }

        Label label = new()
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            AutomationId = AutomationIds.HeaderTitle,
        };
        label.SetBinding(Label.TextProperty, static (BottomSheetHeader header) => header.TitleText, source: this);

        return TryAdd(label, TitleColumn);
    }

    /// <summary>
    /// Arranges the close button in the user interface.
    /// </summary>
    /// <param name="columnToRemove">The column index of the close button to remove.</param>
    /// <remarks>
    /// This method determines the position of the close button based on the value of the <see cref="CloseButtonPosition"/> property.
    /// It then removes the existing close button from the layout, if it exists, and adds a new instance of the close button to the appropriate column.
    /// </remarks>
    private void ArrangeCloseButton(int columnToRemove)
    {
        _ = TryRemove(columnToRemove);

        if (ShowCloseButton)
        {
            TryAdd(CreateCloseButton(), CloseButtonColumn());
        }

        if (this.HasTopLeftButton())
        {
            TryAdd(TopLeftButton, TopLeftButtonColumn);
        }

        if (this.HasTopRightButton())
        {
            TryAdd(TopRightButton, TopRightButtonColumn);
        }
    }

    /// <summary>
    /// Creates and returns a new close button instance.
    /// </summary>
    /// <returns>A new instance of a close button.</returns>
    private CloseButton CreateCloseButton()
    {
        CloseButton closeButton = new(CloseButtonHorizontalOptions())
        {
            AutomationId = AutomationIds.HeaderCloseButton,
            VerticalOptions = LayoutOptions.Start,
        };

        closeButton.Clicked += CloseButtonOnClicked;

        return closeButton;
    }

    /// <summary>
    /// Determines the column index for the Close button in the layout based on its position.
    /// </summary>
    /// <returns>
    /// The column index for the Close button. Returns 0 if the Close button is positioned in the top-left
    /// (TopLeft), or 2 if it is positioned in the top-right (TopRight).
    /// </returns>
    private int CloseButtonColumn() => CloseButtonPosition == BottomSheetHeaderCloseButtonPosition.TopLeft ? TopLeftButtonColumn : TopRightButtonColumn;

    /// Determines the horizontal layout options for the close button within the bottom sheet header.
    /// The method evaluates the current `CloseButtonPosition`
    /// property and returns the corresponding horizontal layout
    /// option, which could either be aligned to the start (left)
    /// or the end (right).
    /// <return>
    /// A `LayoutOptions` value specifying the horizontal alignment
    /// of the close button (either `LayoutOptions.Start` or `LayoutOptions.End`).
    /// </return>
    private LayoutOptions CloseButtonHorizontalOptions() => CloseButtonPosition == BottomSheetHeaderCloseButtonPosition.TopLeft ? LayoutOptions.Start : LayoutOptions.End;

    /// <summary>
    /// Handles the "Clicked" event of the close button in the bottom sheet header.
    /// </summary>
    /// <param name="sender">The source of the event, typically the close button.</param>
    /// <param name="e">The event data associated with the button click.</param>
    private void CloseButtonOnClicked(object? sender, EventArgs e)
    {
        _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CloseButtonClicked));
    }
}