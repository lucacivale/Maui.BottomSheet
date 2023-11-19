namespace Maui.BottomSheet.Navigation;

public interface IBottomSheetStack
{
    IBottomSheet Current { get; }
    bool IsEmpty { get; }

    void Add(IBottomSheet bottomSheet);
    IBottomSheet Remove();
    void Clear();
}