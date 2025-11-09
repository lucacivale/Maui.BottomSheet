namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.Extensions;

public static class ShellExtensions
{
    public static Task GoToBottomSheetHeaderTestsAsync(this Shell shell)
    {
        return shell.GoToAsync(Routes.BottomSheetHeaderTests);
    }
    
    public static Task GoToBottomSheetTestsAsync(this Shell shell)
    {
        return shell.GoToAsync(Routes.BottomSheetTests);
    }
    
    public static Task GoToModalPageBottomSheetTests(this Shell shell)
    {
        return shell.GoToAsync(Routes.ModalPageBottomSheetTests);
    }
}