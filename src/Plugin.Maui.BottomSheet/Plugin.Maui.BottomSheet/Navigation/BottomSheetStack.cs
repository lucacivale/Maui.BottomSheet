namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Manages a collection of bottom sheets using stack-based navigation.
/// </summary>
/// <inheritdoc />
internal sealed class BottomSheetStack : Stack<IBottomSheet>
{
    /// <summary>
    /// Gets the topmost bottom sheet in the stack.
    /// </summary>
    public IBottomSheet Current => Peek();

    /// <summary>
    /// Gets whether the stack contains no bottom sheets.
    /// </summary>
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Pushes a bottom sheet onto the top of the stack.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet to add.</param>
    public void Add(IBottomSheet bottomSheet)
    {
        Push(bottomSheet);
    }

    /// <summary>
    /// Pops and returns the topmost bottom sheet from the stack.
    /// </summary>
    /// <returns>The removed bottom sheet.</returns>
    public IBottomSheet Remove()
    {
        return Pop();
    }
}