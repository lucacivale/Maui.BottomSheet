using System.Diagnostics;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;

public class EmptyBottomSheet : Plugin.Maui.BottomSheet.BottomSheet
{
    public bool IsUnloaded { get; set; }

    public EmptyBottomSheet()
    {
        Content = new()
        {
            Content = new EmptyView()
        };
        Loaded += (_, _) => { Debug.WriteLine("Loaded"); }; 
        Unloaded += (_, _) => { IsUnloaded = true; }; 
    }
}