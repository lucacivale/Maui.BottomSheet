namespace Plugin.Maui.BottomSheet;

using global::Maui.BindableProperty.Generator.Core;

/// <summary>
/// The peek is the first view in the <see cref="IBottomSheet"/> content.
/// Use this to show a small view until the user decides to expand the <see cref="IBottomSheet"/>.
/// Additional content is added to the end of the peek view.
/// </summary>
public sealed partial class BottomSheetPeek : BindableObject
{
    /// <summary>
    /// Gets or sets if the bottom safe area is ignored. If ignored the peek view can be inside the safe area.
    /// If not peek view will be above the safe area.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private bool _ignoreSafeArea;

    /// <summary>
    /// Gets or sets peek height.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private double _peekHeight;

    /// <summary>
    /// Gets or sets peek view <see cref="DataTemplate"/>.
    /// If no <see cref="Views.BottomSheetPeek.PeekHeight"/> is set the peek height will be calculated based on the peek view content.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private DataTemplate? peekViewDataTemplate;
}