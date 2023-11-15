namespace Maui.BottomSheet.Samples.Services;

public class DialogService : IDialogService
{
	public Task DisplayAlert(string title, string message, string cancel)
	{
		return Application.Current.MainPage.DisplayAlert(title, message, cancel);
	}
}

