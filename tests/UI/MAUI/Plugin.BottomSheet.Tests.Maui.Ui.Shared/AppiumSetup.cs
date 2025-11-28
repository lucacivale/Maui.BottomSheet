// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui;

public sealed partial class AppiumSetup
{
    private static string GetPlatformOutputFolderPath(string platform)
    {
        const string testApplicationProjectDirectoryName = "Plugin.BottomSheet.Tests.Maui.Ui.Application";

        DirectoryInfo? mauiDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName)!.FullName)!.FullName);
        
        string testApplicationProjectPath = Path.Combine(mauiDirectory!.FullName, testApplicationProjectDirectoryName);
        
        #if NET9_0
        string framework = $"net9.0-{platform}";
        #else
        string framework = $"net10.0-{platform}";
        #endif

        #if DEBUG
        return Path.Combine(testApplicationProjectPath, "bin", "Debug", framework);
        #else
        return Path.Combine(testApplicationProjectPath, "bin", "Release", framework);
        #endif
    }
}