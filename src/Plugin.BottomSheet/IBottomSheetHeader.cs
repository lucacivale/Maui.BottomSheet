namespace Plugin.BottomSheet;

/// <summary>
/// Defines the contract for a header component displayed within a bottom sheet view.
/// It provides additional properties and customization options such as title text,
/// buttons, and close button configurations.
/// </summary>
internal interface IBottomSheetHeader : IBottomSheetContentView
{
    /// <summary>
    /// Gets the text displayed as the title in the bottom sheet header.
    /// </summary>
    /// <remarks>
    /// The <c>TitleText</c> property determines the textual content shown at the top of the
    /// bottom sheet. It can be used to provide descriptive context or branding for the sheet's content.
    /// Setting this property to <c>null</c> or an empty string will result in no title being displayed.
    /// </remarks>
    string? TitleText { get; }

    /// <summary>
    /// Gets the button displayed in the top-left corner of the bottom sheet header.
    /// This property is used to define a custom button or control that can be displayed
    /// in the specified top-left location within the header region.
    /// </summary>
    object? TopLeftButton { get; }

    /// <summary>
    /// Gets the button displayed in the top-right corner of the bottom sheet header.
    /// </summary>
    /// <remarks>
    /// The <c>TopRightButton</c> property determines the visual and functional content of the top-right button
    /// in the bottom sheet header. This property can be used to assign custom behaviors or visuals to the button
    /// based on user requirements. If not set, no button will be displayed in the top-right corner.
    /// </remarks>
    object? TopRightButton { get; }

    /// <summary>
    /// Gets the position of the close button within the bottom sheet header.
    /// </summary>
    /// <remarks>
    /// The property determines whether the close button is positioned on the top-left
    /// or top-right corner of the bottom sheet header. The position may influence the layout
    /// and design of the header based on the selected value from the <see cref="BottomSheetHeaderCloseButtonPosition"/> enumeration.
    /// </remarks>
    BottomSheetHeaderCloseButtonPosition CloseButtonPosition { get; }

    /// <summary>
    /// Gets a value indicating whether the close button is displayed
    /// on the bottom sheet header.
    /// </summary>
    /// <remarks>
    /// When set to <c>true</c>, a close button is rendered in the location specified
    /// by the <see cref="CloseButtonPosition"/> property. When set to <c>false</c>,
    /// the close button is not displayed regardless of the configured position.
    /// </remarks>
    bool ShowCloseButton { get; }

    /// <summary>
    /// Gets the appearance mode of the header buttons in the bottom sheet.
    /// Determines whether buttons are displayed on the left, right, both sides, or not displayed at all.
    /// </summary>
    BottomSheetHeaderButtonAppearanceMode HeaderAppearance { get; }
}