using CoreGraphics;
using Maui.BottomSheet.Platforms.iOS;
using Microsoft.Maui.Platform;
using System.Diagnostics.CodeAnalysis;
using UIKit;

namespace Maui.BottomSheet;

public class MauiBottomSheet : UIView
{
	#region Members
	private readonly IMauiContext mauiContext;
	private BottomSheetUIViewController? bottomSheetUIViewController;
	private View? peekView;
	#endregion

	#region Properties
	public IBottomSheet? VirtualView { get; private set; }
	#endregion

	#region Constructor
	public MauiBottomSheet(IMauiContext mauiContext)
	{
		this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
	}
	#endregion

	#region Mappers
	public async Task SetIsOpen()
	{
		if (VirtualView?.IsOpen == true)
		{
			await OpenBottomSheet();
		}
		else if (VirtualView?.IsOpen == false)
		{
			await DismissBottomSheet();
		}
	}

	public void SetIsDraggable()
	{
		bottomSheetUIViewController?.EnableDragging(VirtualView?.IsDraggable == true);
	}

	public void SetTitleText()
	{
		SetTitleText(VirtualView?.TitleText ?? string.Empty);
	}

	public void SetTopLeftText()
	{
		if (VirtualView?.HasTopLeft() == true)
		{
			if (bottomSheetUIViewController?.NavigationBar.TopItem.LeftBarButtonItem is null)
			{
				CreateTopLeft();
			}
			else
			{
				bottomSheetUIViewController.NavigationBar.TopItem.LeftBarButtonItem.Title = VirtualView?.TopLeftButtonText ?? "N/a";
			}
		}
	}

	public void SetTopRightText()
	{
		if (VirtualView?.HasTopRight() == true)
		{
			if (bottomSheetUIViewController?.NavigationBar.TopItem.RightBarButtonItem is null)
			{
				CreateTopRight();
			}
			else
			{
				bottomSheetUIViewController.NavigationBar.TopItem.RightBarButtonItem.Title = VirtualView?.TopRightButtonText ?? "N/a";
			}
		}
	}


	public void SetHeaderAppearance()
	{
		if (bottomSheetUIViewController is null)
		{
			return;
		}

		if (VirtualView?.HasTopRight() == false)
		{
			bottomSheetUIViewController.NavigationBar.TopItem.RightBarButtonItem = null;
		}
		else
		{
			CreateTopRight();
		}

		if (VirtualView?.HasTopLeft() == false)
		{
			bottomSheetUIViewController.NavigationBar.TopItem.LeftBarButtonItem = null;
		}
		else
		{
			CreateTopLeft();
		}
	}

	public void SetHeader()
	{
		if (VirtualView?.ShowHeader == true)
		{
			CreateHeader();
			bottomSheetUIViewController?.SetNavigationBarHidden(false, true);
		}
		else if (bottomSheetUIViewController is BottomSheetUIViewController uiViewController)
		{
			uiViewController.NavigationBar.TopItem.LeftBarButtonItem = null;
			uiViewController.NavigationBar.TopItem.RightBarButtonItem = null;
			uiViewController.NavigationBar.TopItem.Title = null;

			uiViewController.NavigationBar.TopItem.TitleView = null;

			uiViewController.SetNavigationBarHidden(true, true);
		}

		SetSheetStates();
	}

	public void SetHasHandle()
	{
		if (bottomSheetUIViewController?.SheetPresentationController is null)
		{
			return;
		}

		bottomSheetUIViewController.SheetPresentationController.PrefersGrabberVisible = VirtualView?.HasHandle == true;
	}

	public void SetSheetStates()
	{
		if (bottomSheetUIViewController?.SheetPresentationController is null
			|| VirtualView is null)
		{
			return;
		}

		var detents = VirtualView.Detents();

		if (CreatePeekDetent() is UISheetPresentationControllerDetent detent)
		{
			detents.Add(detent);
		}

		bottomSheetUIViewController.SheetPresentationController.Detents = detents.ToArray();
	}

	public void SetSelectedSheetState()
	{
		if (bottomSheetUIViewController?.SheetPresentationController is null
			|| VirtualView is null)
		{
			return;
		}

		bottomSheetUIViewController.SheetPresentationController.AnimateChanges(() =>
		{
			bottomSheetUIViewController.SheetPresentationController.SelectedDetentIdentifier = VirtualView.SelectedSheetState.ToPlatform();
		});

	}

	public void SetPeek()
	{
		SetSheetStates();
	}

	#endregion

	public override async void MovedToWindow()
	{
		base.MovedToWindow();

		if (bottomSheetUIViewController?.IsBeingPresented == false)
		{
			await SetIsOpen();
		}
	}

	public void SetView(IBottomSheet element)
	{
		VirtualView = element;
	}

	public void Cleanup()
	{
		if (VirtualView is null)
		{
			return;
		}

		VirtualView = null;
		bottomSheetUIViewController?.Dispose();
		bottomSheetUIViewController = null;
		Dispose();
	}

	#region Creation

	private BottomSheetUIViewController CreateBottomSheet()
	{
		CreateBottomSheetUIViewController();

		if (bottomSheetUIViewController?.SheetPresentationController is UISheetPresentationController sheet)
		{
			ConfigureSheet(sheet);
		}

		if (VirtualView?.ShowHeader == true)
		{
			CreateHeader();
		}

		return bottomSheetUIViewController ?? throw new ArgumentNullException(nameof(bottomSheetUIViewController));
	}

	private void ConfigureSheet(UISheetPresentationController sheet)
	{
		SetSheetStates();

		sheet.LargestUndimmedDetentIdentifier = UISheetPresentationControllerDetentIdentifier.Unknown;
		sheet.PrefersGrabberVisible = VirtualView?.HasHandle == true;
		sheet.PrefersScrollingExpandsWhenScrolledToEdge = true;
	}

	private UISheetPresentationControllerDetent? CreatePeekDetent()
	{
		if (VirtualView?.Peek is null)
		{
			return null;
		}

		if (OperatingSystem.IsIOSVersionAtLeast(16) == false)
		{
			return UISheetPresentationControllerDetent.CreateMediumDetent();
		}

		const string peekDetentId = "99";
		var height = VirtualView?.Peek?.PeekHeight ?? double.NaN;

		if (double.IsNaN(height)
			&& peekView is not null)
		{
			var sizeReq = peekView.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);

			height = sizeReq.Request.Height;
		}

		if (VirtualView?.Peek?.IgnoreSafeArea == true)
		{
			height -= Window.SafeAreaInsets.Bottom;
		}

		if (VirtualView?.ShowHeader == true)
		{
			height += bottomSheetUIViewController?.NavigationBar.Frame.Size.Height ?? height;
		}

		height += VirtualView?.Margin.Top ?? 0;

#pragma warning disable CA1416 // Validate platform compatibility
		return UISheetPresentationControllerDetent.Create(peekDetentId, (context) => new nfloat(height));
#pragma warning restore CA1416 // Validate platform compatibility
	}

	public void CreateBottomSheetUIViewController()
	{
		ContentPage bottomSheet = new();

		View? content = default;
		View? contentView = (View?)VirtualView?.ContentTemplate?.CreateContent();
		peekView = (View?)VirtualView?.Peek?.PeekViewDataTemplate?.CreateContent();

		if (peekView is not null)
		{
			double bottomMargin = peekView.Margin.Bottom;

			UIEdgeInsets? safeAreaInsets = WindowStateManager.Default.GetCurrentUIWindow()?.SafeAreaInsets;

			if (safeAreaInsets is UIEdgeInsets insets)
			{
				bottomMargin += VirtualView?.Peek?.IgnoreSafeArea == true ? -insets.Bottom : insets.Bottom;
			}

			peekView.Margin = new Thickness(
				peekView.Margin.Left,
				peekView.Margin.Top,
				peekView.Margin.Right,
				bottomMargin);

		}
		if (contentView is not null
			&& peekView is not null)
		{
			content = new VerticalStackLayout()
			{
				{ peekView },
				{ contentView },
			};
		}
		else if (contentView is not null)
		{
			content = contentView;
		}
		else if (peekView is not null)
		{
			content = peekView;
		}

		if (content is not null)
		{
			content.Margin = VirtualView?.Margin ?? new Thickness(0);
		}

		bottomSheet.BindingContext = VirtualView?.BindingContext ?? null;
		bottomSheet.Content = content;

		bottomSheetUIViewController = new BottomSheetUIViewController(VirtualView, bottomSheet.ToUIViewController(mauiContext));

		if (bottomSheetUIViewController.SheetPresentationController is not null)
		{
			bottomSheetUIViewController.SheetPresentationController.Delegate = new BottomSheetControllerDelegate(VirtualView);
		}
	}

	#region Header

	private void CreateHeader()
	{
		if (bottomSheetUIViewController is null)
		{
			return;
		}

		SetTitle();

		if (VirtualView?.HasTopRight() == true)
		{
			CreateTopRight();
		}

		 if (VirtualView?.HasTopLeft() == true)
		{
			CreateTopLeft();
		}

	}

	#region Top Right

	private void CreateTopRight()
	{
		if (bottomSheetUIViewController is null)
		{
			return;
		}

		bottomSheetUIViewController.NavigationBar.TopItem.RightBarButtonItem = new UIBarButtonItem(
			VirtualView?.TopRightButtonText ?? string.Empty,
			UIBarButtonItemStyle.Plain,
			TopRightButton_Clicked);
	}

	private void TopRightButton_Clicked(object? sender, EventArgs e)
	{
		if (VirtualView?.TopRightButtonCommand?.CanExecute(VirtualView.TopRightButtonCommandParameter) == true)
		{
			VirtualView.TopRightButtonCommand?.Execute(VirtualView.TopRightButtonCommandParameter);
		}
	}

	#endregion

	#region Top Left

	private void CreateTopLeft()
	{
		if (bottomSheetUIViewController is null)
		{
			return;
		}

		bottomSheetUIViewController.NavigationBar.TopItem.LeftBarButtonItem = new UIBarButtonItem(
			VirtualView?.TopLeftButtonText ?? string.Empty,
			UIBarButtonItemStyle.Plain,
			TopLefttButton_Clicked);
	}

	private void TopLefttButton_Clicked(object? sender, EventArgs e)
	{
		if (VirtualView?.TopLeftButtonCommand?.CanExecute(VirtualView.TopLeftButtonCommandParameter) == true)
		{
			VirtualView.TopLeftButtonCommand?.Execute(VirtualView.TopLeftButtonCommandParameter);
		}
	}

	#endregion

	#region Title

	private void SetTitle()
	{
		if (((View?)VirtualView?.TitleViewTemplate?.CreateContent())?.ToPlatform(mauiContext) is UIView titleView)
		{
			titleView.SizeToFit();

			ArgumentNullException.ThrowIfNull(bottomSheetUIViewController);

			bottomSheetUIViewController.NavigationBar.TopItem.TitleView = titleView;
		}
		else
		{
			SetTitleText(VirtualView?.TitleText ?? string.Empty);
		}
	}
	private void SetTitleText(string text)
	{
		if (bottomSheetUIViewController is null)
		{
			return;
		}

		bottomSheetUIViewController.NavigationBar.TopItem.Title = string.IsNullOrWhiteSpace(text) ? null : text;
	}

	#endregion

	#endregion

	#endregion

	public async Task OpenBottomSheet()
	{
		if (WindowStateManager.Default.GetCurrentUIViewController() is UIViewController parent)
		{
			await parent.PresentViewControllerAsync(CreateBottomSheet(), animated: true);
		}
	}

	public async Task DismissBottomSheet()
	{
		if (bottomSheetUIViewController is not null)
		{
			await bottomSheetUIViewController.DismissViewControllerAsync(true);
		}
	}
}

