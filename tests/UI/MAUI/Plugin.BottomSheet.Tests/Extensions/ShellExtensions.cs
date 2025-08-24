namespace Plugin.BottomSheet.Tests.Extensions;

public static class ShellExtensions
{
    public static Task GoToBottomSheetHeaderTestsAsync(this Shell shell)
    {
        return shell.GoToAsync(Routes.BottomSheetHeaderTests);
    }
}