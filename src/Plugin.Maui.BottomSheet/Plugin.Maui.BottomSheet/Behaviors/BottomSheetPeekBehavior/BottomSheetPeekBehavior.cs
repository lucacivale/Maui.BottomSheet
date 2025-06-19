namespace Plugin.Maui.BottomSheet.Behaviors.BottomSheetPeekBehavior;

/// <summary>
/// Represents a behavior that enables a bottom sheet to display a "peek" interaction on supported platforms.
/// </summary>
/// <remarks>
/// BottomSheetPeekBehavior is a platform-specific behavior that allows you to define a partial visibility state
/// for a bottom sheet. It enables users to view a snippet or preview of the sheet's content without fully opening it.
/// </remarks>
public sealed partial class BottomSheetPeekBehavior : PlatformBehavior<View>;