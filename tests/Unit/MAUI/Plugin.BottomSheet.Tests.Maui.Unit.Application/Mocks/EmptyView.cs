using System.Diagnostics;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;

public class EmptyView : BoxView
{
    public bool IsUnloaded { get; set; }
    
    public EmptyView()
    {
        Loaded += (_, _) => { Debug.WriteLine("Loaded"); }; 
        Unloaded += (_, _) => { IsUnloaded = true; }; 
    }
}
