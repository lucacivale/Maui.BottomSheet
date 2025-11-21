namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Maintains a stack of bottom sheets to facilitate stack-based navigation for UI interactions.
/// </summary>
/// <remarks>
/// This class extends the <see cref="Stack{T}"/> to manage bottom sheets, where each bottom sheet is
/// represented by an implementation of the <see cref="IBottomSheet"/> interface.
/// </remarks>
internal sealed class BottomSheetStack : Stack<IBottomSheet>
{
    /// <summary>
    /// Gets the bottom sheet at the top of the stack without removing it.
    /// </summary>
    public IBottomSheet Current => Peek();

    /// <summary>
    /// Gets a value indicating whether the bottom sheet stack is empty.
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
    /// Removes and returns the topmost bottom sheet from the stack.
    /// </summary>
    /// <returns>The removed bottom sheet.</returns>
    public IBottomSheet Remove()
    {
        return Pop();
    }
}