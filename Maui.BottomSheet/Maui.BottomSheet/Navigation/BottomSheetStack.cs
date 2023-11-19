namespace Maui.BottomSheet.Navigation;

public class BottomSheetStack : IBottomSheetStack
{
    private readonly Stack<IBottomSheet> _bottomSheetStack;
    public IBottomSheet Current => _bottomSheetStack.Peek();
    public bool IsEmpty => _bottomSheetStack.Count == 0;

    public BottomSheetStack()
    {
        _bottomSheetStack = new Stack<IBottomSheet>();
    }

    public void Add(IBottomSheet bottomSheet)
    {
        _bottomSheetStack.Push(bottomSheet);
    }
    public IBottomSheet Remove()
    {
        return _bottomSheetStack.Pop();
    }

    public void Clear()
    {
        _bottomSheetStack.Clear();
    }
}