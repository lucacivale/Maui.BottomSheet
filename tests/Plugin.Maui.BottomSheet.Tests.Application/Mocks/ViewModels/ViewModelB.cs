using System.Diagnostics;
using System.Windows.Input;

namespace Plugin.Maui.BottomSheet.Tests.Application.Mocks.ViewModels;

internal sealed class ViewModelB
{
    public string Title { get; set; } = "I'm title B";
    
    public string TopLeftText { get; set; } = "I'm B top left";
    
    public string TopRightText { get; set; } = "I'm B top right";

    public ICommand TopLeftCommand { get; } = new Command(() => Debug.WriteLine("B Top left clicked."));

    public ICommand TopRightCommand { get; } = new Command(() => Debug.WriteLine("B Top right clicked."));
}