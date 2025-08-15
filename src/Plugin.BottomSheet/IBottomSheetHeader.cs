namespace Plugin.BottomSheet;

internal interface IBottomSheetHeader : IBottomSheetContentView
{
    string? TitleText { get; }

    object? TopLeftButton { get; }

    object? TopRightButton { get; }

    BottomSheetHeaderCloseButtonPosition CloseButtonPosition { get; }

    bool ShowCloseButton { get; }

    BottomSheetHeaderButtonAppearanceMode HeaderAppearance { get; }
}