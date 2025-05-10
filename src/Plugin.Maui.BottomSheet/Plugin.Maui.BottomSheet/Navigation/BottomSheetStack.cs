namespace Plugin.Maui.BottomSheet.Navigation;

/// <inheritdoc />
internal sealed class BottomSheetStack : Stack<IBottomSheet>
{
    /// <summary>
    /// Gets current <see cref="IBottomSheet"/>.
    /// </summary>
    public IBottomSheet Current => Peek();

    /// <summary>
    /// Gets a value indicating whether the stack is empty.
    /// </summary>
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Add a sheet to the stack.
    /// </summary>
    /// <param name="bottomSheet">Sheet to add.</param>
    public void Add(IBottomSheet bottomSheet)
    {
        Push(bottomSheet);
    }

    /// <summary>
    /// Remove last <see cref="IBottomSheet"/> from the stack.
    /// </summary>
    /// <returns>The removed <see cref="IBottomSheet"/>.</returns>
    public IBottomSheet Remove()
    {
        return Pop();
    }
}