using System.Diagnostics;
using System.Windows.Input;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks.ViewModels;

internal sealed class ViewModelA
{
    public string Title { get; set; } = "I'm title A";
    
    public string TopLeftText { get; set; } = "I'm A top left";
    
    public string TopRightText { get; set; } = "I'm A top right";

    public ICommand TopLeftCommand { get; } = new Command(() => Debug.WriteLine("A Top left clicked."));

    public ICommand TopRightCommand { get; } = new Command(() => Debug.WriteLine("A Top right clicked."));
}