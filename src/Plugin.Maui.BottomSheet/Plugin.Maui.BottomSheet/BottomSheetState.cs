namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Allowed <see cref="IBottomSheet"/> states.
/// </summary>
[Flags]
public enum BottomSheetState
{
    /// <summary>
    /// No state is allowed.
    /// </summary>
    None = 0,

    /// <summary>
    /// Only <see cref="Peek"/> state is allowed. <see cref="IBottomSheet"/> height is set by <see cref="Peek"/>.
    /// </summary>
    Peek = 1,

    /// <summary>
    /// Only <see cref="Medium"/> state is allowed. <see cref="IBottomSheet"/> height is half screen size.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// Only <see cref="Large"/> state is allowed. <see cref="IBottomSheet"/> height is screen size.
    /// </summary>
    Large = 4,
}