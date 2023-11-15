using Android.Content;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using Maui.BottomSheet.Platforms.Android;
using Microsoft.Maui.Platform;
using MauiView = Microsoft.Maui.Controls.View;
using AndroidView = Android.Views.View;
using Android.Util;
using AndroidButton = Android.Widget.Button;
using AndroidX.Core.Content;
using Microsoft.Maui.Controls.Shapes;

namespace Maui.BottomSheet;

public class MauiBottomSheet : AndroidView
{
	#region Members
	private LinearLayout? sheetLayout;
	private AndroidButton? topLeftButton;
	private AndroidButton? topRightButton;
	private AndroidView? titleView;
	private LinearLayout? headerView;
	private MauiView? peekView;
	private LinearLayout? sheetContainer;
	private AndroidView? handle;
	readonly IMauiContext mauiContext;
	private BottomSheetDialog? bottomSheetDialog;
	#endregion

	#region Properties
	public IBottomSheet? VirtualView { get; private set; }
	#endregion

	#region Constructors
	public MauiBottomSheet(IMauiContext mauiContext, Context context) : base(context)
	{
		this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
	}
	#endregion

	#region Mappers
	public void SetIsOpen()
	{
		if (VirtualView?.IsOpen == true)
		{
			OpenBottomSheet();
		}
		else
		{
			DismissBottomSheet();
		}
	}

	public void SetIsDraggable()
	{
		if (bottomSheetDialog is null)
		{
			return;
		}

		bottomSheetDialog.Behavior.Draggable = VirtualView?.IsDraggable == true;
	}

	public void SetTitleText()
	{
		if (titleView is TextView textView)
		{
			textView.Text = VirtualView?.TitleText ?? "N/a";
		}
	}

	public void SetTopLeftText()
	{
		if (topLeftButton is not null)
		{
			topLeftButton.Text = VirtualView?.TitleText ?? "N/a";
		}
	}

	public void SetTopRightText()
	{
		if (topRightButton is not null)
		{
			topRightButton.Text = VirtualView?.TitleText ?? "N/a";
		}
	}

	public void SetHeaderAppearance()
	{
		if (bottomSheetDialog is null)
		{
			return;
		}

		if (VirtualView?.HasTopLeft() == false)
		{
			RemoveView(topLeftButton);
		}
		else if (topLeftButton?.Parent is null)
		{
			headerView?.AddView(topLeftButton, 0);
		}

		if (VirtualView?.HasTopRight() == false)
		{
			RemoveView(topRightButton);
		}
		else if (topRightButton?.Parent is null)
		{
			headerView?.AddView(topRightButton);
		}
	}

	private static void RemoveView(AndroidView? view)
	{
		if (view?.Parent is not null)
		{
			view.RemoveFromParent();
		}
	}

	public void SetHeader()
	{
		if (VirtualView?.ShowHeader == true)
		{
			sheetLayout?.Post(() => sheetLayout.AddView(CreateHeader(), 0));
		}
		else
		{
			RemoveView(topLeftButton);
			RemoveView(topRightButton);
			RemoveView(titleView);
			RemoveView(headerView);
		}

		if (bottomSheetDialog is not null)
		{
			bottomSheetDialog.Behavior.PeekHeight = CalcPeekHeight();
		}
	}

	public void SetHasHandle()
	{
		if (VirtualView?.HasHandle == true
			&& (handle is null
				|| handle.Parent is null))
		{
			handle = CreateHandle();
			sheetContainer?.AddView(handle, 0);
		}
		else if (VirtualView?.HasHandle == false)
		{
			RemoveView(handle);
		}

		if (bottomSheetDialog is not null)
		{
			bottomSheetDialog.Behavior.PeekHeight = CalcPeekHeight();
		}
	}

	public void SetSheetStates()
	{
		if (VirtualView is null)
		{
			return;
		}

		SetState(VirtualView.SheetStates);
	}

	public void SetSelectedSheetState()
	{
		if (VirtualView is null)
		{
			return;
		}

		SetState(VirtualView.SelectedSheetState);
	}

	private void SetState(BottomSheetState state)
	{
		if (bottomSheetDialog is null
			|| VirtualView is null)
		{
			return;
		}

		int sheetState = state.ToPlatform();

		if (VirtualView.Peek is not null
			&& (state == BottomSheetState.Unknown
				|| state == BottomSheetState.All))
		{
			sheetState = BottomSheetBehavior.StateCollapsed;
		}

		bottomSheetDialog.Behavior.State = sheetState;
	}

	public void SetPeek()
	{
		if (bottomSheetDialog is null)
		{
			return;
		}

		ConfigureSheet(bottomSheetDialog);
	}

	#endregion

	#region Creation

	private BottomSheetDialog CreateBottomSheet()
	{
		ArgumentNullException.ThrowIfNull(Context);

		sheetContainer = new LinearLayout(Context)
		{
			LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent),
			Orientation = Orientation.Vertical
		};

		handle = CreateHandle();
		sheetContainer.AddView(handle);

		sheetLayout = new LinearLayout(Context)
		{
			LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent)
			{
				TopMargin = Convert.ToInt32(VirtualView?.Margin.Top ?? 0),
				BottomMargin = Convert.ToInt32(VirtualView?.Margin.Bottom ?? 0),
				LeftMargin = Convert.ToInt32(VirtualView?.Margin.Left ?? 0),
				RightMargin = Convert.ToInt32(VirtualView?.Margin.Right ?? 0),
			},
			Orientation = Orientation.Vertical
		};

		if (VirtualView?.ShowHeader == true)
		{
			sheetLayout.AddView(CreateHeader());
		}

		var contentlayout = new LinearLayout(Context)
		{
			LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent),
			Orientation = Orientation.Vertical
		};
		contentlayout.SetMinimumHeight(Resources?.DisplayMetrics?.HeightPixels ?? 0);

		if ((MauiView?)VirtualView?.Peek?.PeekViewDataTemplate?.CreateContent() is MauiView peek)
		{
			peekView = peek;
			peekView.BindingContext = VirtualView.BindingContext;
			contentlayout.AddView(peekView.ToPlatform(mauiContext));
		}

		if (((MauiView?)VirtualView?.ContentTemplate?.CreateContent())is MauiView content)
		{
			content.BindingContext = VirtualView.BindingContext;
			contentlayout.AddView(content?.ToPlatform(mauiContext));
		}

		sheetLayout.AddView(contentlayout);

		sheetContainer.AddView(sheetLayout);
		bottomSheetDialog = new BottomSheetDialog(Context);
		bottomSheetDialog.SetContentView(sheetContainer);

		ConfigureSheet(bottomSheetDialog);

		bottomSheetDialog.DismissEvent += BottomSheetDialog_DismissEvent;

		return bottomSheetDialog ?? throw new ArgumentNullException(nameof(bottomSheetDialog));
	}

	private AndroidView? CreateHandle()
	{
		AndroidView? view = Inflate(Context, Resource.Layout.bottomSheetDragHandle, null);
		if (view is not null)
		{
			view.LayoutParameters ??= new LinearLayout.LayoutParams(50, 10)
			{
				TopMargin = 5,
				Gravity = GravityFlags.CenterHorizontal
			};

			if (Context is not null)
			{
				view.Background = ContextCompat.GetDrawable(Context, Resource.Drawable.round_rect_shape);
			}
		}

		return view;
	}
	private void BottomSheetDialog_DismissEvent(object? sender, EventArgs e)
	{
		if (VirtualView is not null)
		{
			VirtualView.IsOpen = false;
		}
	}

	private void ConfigureSheet(BottomSheetDialog sheet)
	{
		sheet.Behavior.Draggable = VirtualView?.IsDraggable == true;
		sheet.Behavior.SkipCollapsed = VirtualView?.Peek is null;
		sheet.Behavior.FitToContents = false;
		sheet.Behavior.HalfExpandedRatio = 0.5f;
		sheet.Behavior.AddBottomSheetCallback(new BottomSheetCallback(VirtualView, this));
		sheet.Behavior.PeekHeight = CalcPeekHeight();

		SetSelectedSheetState();
	}

	private int CalcPeekHeight()
	{
		double peekheight = VirtualView?.Peek?.PeekHeight ?? 0;

		if (double.IsNaN(peekheight)
			&& peekView is not null)
		{
			var sizeReq = peekView?.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);
			peekheight = sizeReq?.Request.Height ?? 0;

			if (VirtualView?.ShowHeader == true
				&& headerView is not null)
			{
				headerView.Measure(0, 0);
				peekheight += headerView.MeasuredHeight;
			}
		}

		if (handle is not null)
		{
			handle.Measure(0, 0);
			peekheight += handle.MeasuredHeight;
		}

		peekheight += VirtualView?.Margin.Top ?? 0;
		peekheight = TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)peekheight, Context?.Resources?.DisplayMetrics);

		return Convert.ToInt32(Math.Round(peekheight, MidpointRounding.AwayFromZero));
	}

	#region Header

	private LinearLayout CreateHeader()
	{
		var headerLayout = new LinearLayout(Context)
		{
			LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent),
			Orientation = Orientation.Horizontal
		};

		if (VirtualView?.HasTopLeft() == true)
		{
			topLeftButton = CreateTopLeft();
			headerLayout.AddView(topLeftButton);
		}

		AndroidView? title = CreateTitleView();

		if (title is not null)
		{
			titleView = title;
			headerLayout.AddView(titleView);
		}

		if (VirtualView?.HasTopRight() == true)
		{
			topRightButton = CreateTopRight();
			headerLayout.AddView(topRightButton);
		}

		headerLayout.Id = GenerateViewId();
		headerView = headerLayout;

		return headerLayout;
	}

	private AndroidView? CreateTitleView()
	{
		if (VirtualView?.HasTitle() == false
			&& VirtualView?.TitleViewTemplate is null)
		{
			return null;
		}

		var layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent)
		{
			Gravity = GravityFlags.Center,
			Weight = 2,
			Width = 0,
		};

		AndroidView? view = null;

		if (VirtualView?.HasTitle() == true)
		{
			TextView title = new(Context)
			{
				LayoutParameters = layoutParams,
				Text = VirtualView?.TitleText,
				TextAlignment = Android.Views.TextAlignment.Center,
				Id = GenerateViewId()
			};
			title.SetTypeface(title.Typeface, Android.Graphics.TypefaceStyle.Bold);
			view = title;
		}
		else if (((MauiView?)VirtualView?.TitleViewTemplate?.CreateContent())  is MauiView mauiTitleView)
		{
			AndroidView? titleView = mauiTitleView.ToPlatform(mauiContext);
			titleView.LayoutParameters = layoutParams;
			view = titleView;
		}

		return view;
	}
	private AndroidButton CreateButtonView(GravityFlags gravityFlags, string text, EventHandler clickEvent)
	{
		var layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
		{
			Gravity = gravityFlags,
			Weight = 1,
			Width = 0,
		};

		AndroidButton button = new(Context)
		{
			LayoutParameters = layoutParams,
			Text = text,
			Id = GenerateViewId()
		};

		button.Click += clickEvent; 

		return button;
	}

	private AndroidButton CreateTopLeft()
	{
		return CreateButtonView(
			GravityFlags.Left,
			VirtualView?.TopLeftButtonText ?? "N/a",
			TopLeftButton_Click);
	}

	private void TopLeftButton_Click(object? sender, EventArgs e)
	{
		if (VirtualView?.TopLeftButtonCommand?.CanExecute(VirtualView?.TopLeftButtonCommandParameter) == true)
		{
			VirtualView?.TopLeftButtonCommand?.Execute(VirtualView?.TopLeftButtonCommandParameter);
		}
	}

	private AndroidButton CreateTopRight()
	{
		return CreateButtonView(
			GravityFlags.Right,
			VirtualView?.TopRightButtonText ?? "N/a",
			TopRightButton_Click);
	}

	private void TopRightButton_Click(object? sender, EventArgs e)
	{
		if (VirtualView?.TopRightButtonCommand?.CanExecute(VirtualView?.TopRightButtonCommandParameter) == true)
		{
			VirtualView?.TopRightButtonCommand?.Execute(VirtualView?.TopRightButtonCommandParameter);
		}
	}
	#endregion

	#endregion

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

		sheetLayout?.Dispose();
		topRightButton?.Dispose();
		topLeftButton?.Dispose();
		titleView?.Dispose();
		headerView?.Dispose();
		sheetContainer?.Dispose();
		handle?.Dispose();
		VirtualView = null;
		bottomSheetDialog = null;
	}

	public void OpenBottomSheet()
	{
		CreateBottomSheet();

		bottomSheetDialog?.Show();
	}

	public void DismissBottomSheet()
	{
		if (bottomSheetDialog is not null)
		{
			bottomSheetDialog.DismissEvent -= BottomSheetDialog_DismissEvent;
			bottomSheetDialog.Dismiss();
		}
	}

	public bool TrySetState(int state)
	{
		if (VirtualView is null)
		{
			return false;
		}

		if (state == BottomSheetBehavior.StateCollapsed
			&& VirtualView?.SheetStates != BottomSheetState.All
			&& VirtualView?.SheetStates != BottomSheetState.Peek)
		{
			return false;
		}

		else if (state == BottomSheetBehavior.StateHalfExpanded
			&& VirtualView?.SheetStates != BottomSheetState.All
			&& VirtualView?.SheetStates != BottomSheetState.Medium)
		{
			return false;
		}

		else if (state == BottomSheetBehavior.StateExpanded
			&& VirtualView?.SheetStates != BottomSheetState.All
			&& VirtualView?.SheetStates != BottomSheetState.Large)
		{
			return false;
		}

		if (state == BottomSheetBehavior.StateCollapsed)
		{
			VirtualView.SelectedSheetState = BottomSheetState.Peek;
		}

		else if (state == BottomSheetBehavior.StateHalfExpanded)
		{
			VirtualView.SelectedSheetState = BottomSheetState.Medium;
		}

		else if (state == BottomSheetBehavior.StateExpanded)
		{
			VirtualView.SelectedSheetState = BottomSheetState.Large;
		}

		return true;
	}
}

