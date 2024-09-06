namespace Plugin.Maui.BottomSheet;

using global::Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls;

/// <summary>
/// The header shown at the top of <see cref="IBottomSheet"/>.
/// </summary>
public sealed partial class BottomSheetHeader : BindableObject
{
    /// <summary>
    /// Gets or sets title text.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private string? _titleText;

    /// <summary>
    /// Gets or sets title <see cref="View"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private View? _titleView;

    /// <summary>
    /// Gets or sets title <see cref="DataTemplate"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private DataTemplate? _titleDataTemplate;

    /// <summary>
    /// Gets or sets the <see cref="Button"/> at the top left in the <see cref="BottomSheetHeader"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private Button? _topLeftButton;

    /// <summary>
    /// Gets or sets the <see cref="Button"/> at the top right in the <see cref="BottomSheetHeader"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private Button? _topRightButton;

    /// <summary>
    /// Gets or sets a custom header <see cref="View"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private View? _headerView;

    /// <summary>
    /// Gets or sets the <see cref="BottomSheetHeaderButtonAppearanceMode"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0169:Field is never used.", Justification = "Field used for code generation.")]
    [AutoBindable]
    private BottomSheetHeaderButtonAppearanceMode _headerAppearance;
}