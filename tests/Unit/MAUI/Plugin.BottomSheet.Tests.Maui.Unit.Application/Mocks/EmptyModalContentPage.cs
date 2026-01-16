namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;

public class EmptyModalContentPage : ContentPage
{
    public EmptyModalContentPage()
    {
        Shell.SetPresentationMode(this, PresentationMode.Modal);
    }
}