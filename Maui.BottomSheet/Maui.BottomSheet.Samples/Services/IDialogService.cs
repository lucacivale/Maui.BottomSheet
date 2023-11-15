namespace Maui.BottomSheet.Samples.Services;

public interface IDialogService
{
	public Task DisplayAlert(string title, string message, string cancel);
}

