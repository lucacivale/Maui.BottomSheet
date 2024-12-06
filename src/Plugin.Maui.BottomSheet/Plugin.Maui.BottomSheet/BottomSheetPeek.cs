namespace Plugin.Maui.BottomSheet;

/// <summary>
/// The peek is the first view in the <see cref="IBottomSheet"/> content.
/// Use this to show a small view until the user decides to expand the <see cref="IBottomSheet"/>.
/// Additional content is added to the end of the peek view.
/// </summary>
public sealed class BottomSheetPeek : BindableObject
{
    /// <summary>
    /// Bindable property.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty IgnoreSafeAreaProperty =
        BindableProperty.Create(
            nameof(IgnoreSafeArea),
            typeof(bool),
            typeof(BottomSheetPeek));

    /// <summary>
    /// Bindable property.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty PeekHeightProperty =
        BindableProperty.Create(
            nameof(PeekHeight),
            typeof(double),
            typeof(BottomSheetPeek));

    /// <summary>
    /// Bindable property.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty PeekViewDataTemplateProperty =
        BindableProperty.Create(
            nameof(PeekViewDataTemplateProperty),
            typeof(DataTemplate),
            typeof(BottomSheetPeek));

    /// <summary>
    /// Gets or sets a value indicating whether the bottom safe area is ignored. If ignored the peek view can be inside the safe area.
    /// If not peek view will be above the safe area.
    /// </summary>
    public bool IgnoreSafeArea { get => (bool)GetValue(IgnoreSafeAreaProperty); set => SetValue(IgnoreSafeAreaProperty, value); }

    /// <summary>
    /// Gets or sets peek height.
    /// </summary>
    public double PeekHeight { get => (double)GetValue(PeekHeightProperty); set => SetValue(PeekHeightProperty, value); }

    /// <summary>
    /// Gets or sets peek view <see cref="DataTemplate"/>.
    /// If no <see cref="BottomSheetPeek.PeekHeight"/> is set the peek height will be calculated based on the peek view content.
    /// </summary>
    public DataTemplate? PeekViewDataTemplate { get => (DataTemplate?)GetValue(PeekViewDataTemplateProperty); set => SetValue(PeekViewDataTemplateProperty, value); }

    /// <summary>
    /// Gets or sets the parent <see cref="Element"/> of this element.
    /// </summary>
    public Element? Parent { get; set; }
}