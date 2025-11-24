namespace Plugin.BottomSheet;

/// <summary>
/// Represents the base interface for defining the content displayed in a bottom sheet component.
/// </summary>
internal interface IBottomSheetContentView
{
    /// <summary>
    /// Gets the content of the bottom sheet view.
    /// </summary>
    /// <remarks>
    /// The content property represents the primary visual element displayed within
    /// the bottom sheet. It can be assigned to any valid UI view or component
    /// adhering to the expected type.
    /// </remarks>
    object? Content { get; }

    /// <summary>
    /// Gets the template that defines the visual structure of the content within the bottom sheet.
    /// The template is applied to render the content dynamically and can support data binding scenarios.
    /// </summary>
    object? ContentTemplate { get; }
}