using System.Diagnostics.CodeAnalysis;
using Plugin.BottomSheet;

namespace Plugin.Maui.BottomSheet;

using Microsoft.Maui.Controls;

/// <summary>
/// Represents the header section displayed at the top of a bottom sheet.
/// </summary>
public sealed class BottomSheetHeader : BottomSheetContentView, IBottomSheetHeader
{
    /// <summary>
    /// Bindable property for the title text.
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(
            nameof(TitleText),
            typeof(string),
            typeof(BottomSheetHeader),
            propertyChanged: OnTitleTextPropertyChanged);

    /// <summary>
    /// Bindable property for the top left button.
    /// </summary>
    public static readonly BindableProperty TopLeftButtonProperty =
        BindableProperty.Create(
            nameof(TopLeftButton),
            typeof(Button),
            typeof(BottomSheetHeader),
            propertyChanged: OnTopLeftButtonPropertyChanged);

    /// <summary>
    /// Bindable property for the top right button.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty TopRightButtonProperty =
        BindableProperty.Create(
            nameof(TopRightButton),
            typeof(Button),
            typeof(BottomSheetHeader),
            propertyChanged: OnTopRightButtonPropertyChanged);

    /// <summary>
    /// Bindable property for the close button visibility.
    /// </summary>
    public static readonly BindableProperty ShowCloseButtonProperty =
        BindableProperty.Create(
            nameof(ShowCloseButton),
            typeof(bool),
            typeof(BottomSheetHeader),
            propertyChanged: OnShowCloseButtonPropertyChanged);

    /// <summary>
    /// Bindable property for the close button position.
    /// </summary>
    public static readonly BindableProperty CloseButtonPositionProperty =
        BindableProperty.Create(
            nameof(BottomSheetHeaderCloseButtonPosition),
            typeof(BottomSheetHeaderCloseButtonPosition),
            typeof(BottomSheetHeader),
            defaultValue: BottomSheetHeaderCloseButtonPosition.TopRight,
            propertyChanged: OnCloseButtonPositionPropertyChanged);

    /// <summary>
    /// Bindable property for the header button appearance mode.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty HeaderAppearanceProperty =
        BindableProperty.Create(
            nameof(HeaderAppearance),
            typeof(BottomSheetHeaderButtonAppearanceMode),
            typeof(BottomSheetHeader),
            propertyChanged: OnHeaderAppearancePropertyChanged);

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

    public event EventHandler CloseButtonClicked
    {
        add => _weakEventManager.AddEventHandler(value);
        remove => _weakEventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets the title text displayed in the header.
    /// </summary>
    public string? TitleText
    {
        get => (string?)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the button displayed at the top left of the header.
    /// </summary>
    public Button? TopLeftButton
    {
        get => (Button?)GetValue(TopLeftButtonProperty);
        set => SetValue(TopLeftButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the button displayed at the top right of the header.
    /// </summary>
    public Button? TopRightButton
    {
        get => (Button?)GetValue(TopRightButtonProperty);
        set => SetValue(TopRightButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the position of the close button in the header.
    /// </summary>
    public BottomSheetHeaderCloseButtonPosition CloseButtonPosition
    {
        get => (BottomSheetHeaderCloseButtonPosition)GetValue(CloseButtonPositionProperty);
        set => SetValue(CloseButtonPositionProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the close button should be displayed.
    /// </summary>
    public bool ShowCloseButton
    {
        get => (bool)GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    public BottomSheetHeaderStyle Style
    {
        get => (BottomSheetHeaderStyle)GetValue(StyleProperty);
        set => SetValue(StyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the appearance mode for header buttons.
    /// </summary>
    public BottomSheetHeaderButtonAppearanceMode HeaderAppearance
    {
        get => (BottomSheetHeaderButtonAppearanceMode)GetValue(HeaderAppearanceProperty);
        set => SetValue(HeaderAppearanceProperty, value);
    }

    object? IBottomSheetHeader.TopLeftButton => TopLeftButton;

    object? IBottomSheetHeader.TopRightButton => TopRightButton;

    object? IBottomSheetContentView.Content => Content;

    object? IBottomSheetContentView.ContentTemplate => ContentTemplate;

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

    internal override void Remove()
    {
        base.Remove();

        if (_builtInHeaderLayout is not null)
        {
            _builtInHeaderLayout.Parent = null;
            _builtInHeaderLayout.BindingContext = null;
            _builtInHeaderLayout.DisconnectHandlers();
            _builtInHeaderLayout = null;
        }
    }

    protected override void OnParentChanged(Element parent)
    {
        base.OnParentChanged(parent);

        if (_builtInHeaderLayout is not null)
        {
            _builtInHeaderLayout.Parent = parent;
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        OnBindingContextChanged(_builtInHeaderLayout);
    }

    private static void OnTopLeftButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnTopLeftButtonPropertyChanged((Button)newValue);

    private static void OnTopRightButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnTopRightButtonPropertyChanged((Button)newValue);

    private static void OnShowCloseButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnShowCloseButtonPropertyChanged();

    private static void OnCloseButtonPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnCloseButtonPositionPropertyChanged((BottomSheetHeaderCloseButtonPosition)oldValue);

    private static void OnHeaderAppearancePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnHeaderAppearancePropertyChanged();

    private static void OnTitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnTitleTextPropertyChanged((string)oldValue);

    private static void OnStyleChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetHeader)bindable).OnStyleChanged();

    private void OnStyleChanged()
    {
        SetTitleStyle();
        SetCloseButtonStyle();
    }

    private void SetTitleStyle()
    {
        if (this.HasTitle()
            && TryGetView(TitleColumn, out Label? title) == true)
        {
            title.SetBinding(Label.TextColorProperty, static (BottomSheetHeaderStyle style) => style.TitleTextColor, source: Style);
            title.SetBinding(Label.FontSizeProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontSize, source: Style);
            title.SetBinding(Label.FontAttributesProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontAttributes, source: Style);
            title.SetBinding(Label.FontFamilyProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontFamily, source: Style);
            title.SetBinding(Label.FontAutoScalingEnabledProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontAutoScalingEnabled, source: Style);
        }
    }

    private void SetCloseButtonStyle()
    {
        if ((this.HasTopLeftCloseButton()
                || this.HasTopRightCloseButton())
            && TryGetView(CloseButtonColumn(), out CloseButton? closeButton) == true)
        {
            closeButton.SetBinding(CloseButton.TintProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonTintColor, source: Style);
            closeButton.SetBinding(VisualElement.HeightRequestProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonHeightRequest, source: Style);
            closeButton.SetBinding(VisualElement.WidthRequestProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonWidthRequest, source: Style);
        }
    }

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

    private void OnTopLeftButtonPropertyChanged(Button newButton)
    {
        OnTopButtonChanged(newButton, TopLeftButtonColumn, this.HasTopLeftButton());
    }

    private void OnTopRightButtonPropertyChanged(Button newButton)
    {
        OnTopButtonChanged(newButton, TopRightButtonColumn, this.HasTopRightButton());
    }

    private void OnTopButtonChanged(Button newButton, int column, bool addNewButton)
    {
        _ = TryRemove(column);

        if (addNewButton)
        {
            _ = TryAdd(newButton, column);
        }
    }

    private void OnShowCloseButtonPropertyChanged()
    {
        ArrangeCloseButton(CloseButtonColumn());
    }

    private void OnCloseButtonPositionPropertyChanged(BottomSheetHeaderCloseButtonPosition oldValue)
    {
        int column = oldValue == BottomSheetHeaderCloseButtonPosition.TopLeft ? TopLeftButtonColumn : TopRightButtonColumn;

        ArrangeCloseButton(column);
    }

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

    private bool TryAddTitle()
    {
        if (TryGetView(TitleColumn, out View? _) == true)
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

    private int CloseButtonColumn() => CloseButtonPosition == BottomSheetHeaderCloseButtonPosition.TopLeft ? TopLeftButtonColumn : TopRightButtonColumn;

    private LayoutOptions CloseButtonHorizontalOptions() => CloseButtonPosition == BottomSheetHeaderCloseButtonPosition.TopLeft ? LayoutOptions.Start : LayoutOptions.End;

    private void CloseButtonOnClicked(object? sender, EventArgs e)
    {
        _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CloseButtonClicked));
    }
}