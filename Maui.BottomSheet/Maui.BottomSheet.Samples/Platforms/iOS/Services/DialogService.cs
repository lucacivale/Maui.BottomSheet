using Maui.BottomSheet.Platforms.iOS;
using UIKit;

namespace Maui.BottomSheet.Samples.Services;

public class DialogService : IDialogService
{
	public Task DisplayAlert(string title, string message, string cancel)
	{
		if (WindowStateManager.Default.GetCurrentUIViewController() is BottomSheetUIViewController sheet)
		{
			var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

			if (alert.View is not null)
			{
				var oldFrame = alert.View.Frame;

				alert.View.Frame = new RectF((float)oldFrame.X, (float)oldFrame.Y, (float)oldFrame.Width, (float)oldFrame.Height);
			}

			TaskCompletionSource<bool> result = new();

			alert.AddAction(UIAlertAction.Create(cancel, UIAlertActionStyle.Cancel, (alert) => result.TrySetResult(false)));

			sheet.PresentViewController(alert, true, null);

			return result.Task;
		}
		else
		{
			return Shell.Current.CurrentPage.DisplayAlert(title, message, cancel);
		}
	}
}

