using Android.Content;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using Maui.BottomSheet.Platforms.Android;
using Microsoft.Maui.Platform;
using MauiView = Microsoft.Maui.Controls.View;
using AndroidView = Android.Views.View;
using AndroidButton = Android.Widget.Button;
using AndroidX.Core.Content;

namespace Maui.BottomSheet;

using Android.Graphics.Drawables;
using Microsoft.Maui.Controls.Platform;
using System.Windows.Input;

public class MauiBottomSheet : AndroidView
{
	#region Members
	private readonly BottomSheetLayoutChangeListener _layoutChangeListener;

	private LinearLayout? sheetLayout;
	private AndroidButton? topLeftButton;
	private AndroidButton? topRightButton;
	private AndroidView? titleView;
	private LinearLayout? headerView;
	private AndroidView? peekView;
	private LinearLayout? sheetContainer;
	private AndroidView? handle;
	readonly IMauiContext mauiContext;
	private BottomSheetDialog? bottomSheetDialog;
	private BottomSheetCallback? bottomSheetCallback;
	#endregion

	#region Properties
	public IBottomSheet? VirtualView { get; private set; }
	#endregion

	#region Constructors
	public MauiBottomSheet(IMauiContext mauiContext, Context context) : base(context)
	{
		this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
		_layoutChangeListener = new(this);
	}
	#endregion

	#region Mappers
	public void SetIsCancelable()
	{
		bottomSheetDialog?.SetCancelable(VirtualView?.IsCancelable == true);
	}

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
    
	public void SetBackgroundColor()
	{
		if (VirtualView?.BackgroundColor is not null
		    && sheetContainer is not null)
		{
			GradientDrawable shape = new();
			shape.SetColor(VirtualView.BackgroundColor.ToPlatform());
			sheetContainer.SetBackground(shape);
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

		bottomSheetDialog.Behavior.PeekHeight = CalcPeekHeight();

		SetSelectedSheetState();
	}

	#endregion

	#region Creation

	private BottomSheetDialog CreateBottomSheet()
	{
		ArgumentNullException.ThrowIfNull(Context);

		sheetContainer = new LinearLayout(Context)
		{
			LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent),
			Orientation = Orientation.Vertical,
		};

		handle = CreateHandle();
		handle?.AddOnLayoutChangeListener(_layoutChangeListener);
		sheetContainer.AddView(handle);

		sheetLayout = new LinearLayout(Context)
		{
			LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent)
			{
				TopMargin = (VirtualView?.Margin.Top ?? 0).ToPixels(Context).RoundUpToNextInt(),
				BottomMargin = (VirtualView?.Margin.Bottom ?? 0).ToPixels(Context).RoundUpToNextInt(),
				LeftMargin = (VirtualView?.Margin.Left ?? 0).ToPixels(Context).RoundUpToNextInt(),
				RightMargin = (VirtualView?.Margin.Right ?? 0).ToPixels(Context).RoundUpToNextInt(),
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
			peek.BindingContext = VirtualView.BindingContext;
			peekView = peek.ToPlatform(mauiContext);
			peekView.AddOnLayoutChangeListener(_layoutChangeListener);
			contentlayout.AddView(peekView);
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
			view.LayoutParameters ??= new LinearLayout.LayoutParams(30.ToPixels(Context).RoundUpToNextInt(), 5.ToPixels(Context).RoundUpToNextInt())
			{
				TopMargin = 5.ToPixels(Context).RoundUpToNextInt(),
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
		
		if (bottomSheetCallback is null)
		{
			bottomSheetCallback = new BottomSheetCallback(VirtualView, this);
			sheet.Behavior.AddBottomSheetCallback(bottomSheetCallback);
		}
		sheet.Behavior.PeekHeight = CalcPeekHeight();

		SetSelectedSheetState();
		SetIsCancelable();
		SetBackgroundColor();
    }

	private int CalcPeekHeight()
	{
		double peekheight = (VirtualView?.Peek?.PeekHeight ?? 0).ToPixels(Context);

		if (double.IsNaN(peekheight)
			&& peekView is not null)
		{
			peekheight = peekView.Height;

			if (VirtualView?.ShowHeader == true
				&& headerView is not null)
			{
				peekheight += headerView.Height;
			}
		}

		if (handle is not null)
		{
			peekheight += handle.Height;
		}

		peekheight += (VirtualView?.Margin.Top ?? 0).ToPixels(Context);
		
		return peekheight.RoundUpToNextInt();
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
		headerLayout.AddOnLayoutChangeListener(_layoutChangeListener);

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
			Label title = new()
			{
				HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center,
				VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
				FontSize = 20,
				Text = VirtualView?.TitleText	
			};

			AndroidView androidTitle = title.ToPlatform(mauiContext);
			androidTitle.Id = GenerateViewId();
			androidTitle.LayoutParameters = layoutParams;

			view = androidTitle;
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
			Id = GenerateViewId(),
			Background = null,
		};

		if (Context is not null)
		{
			var color = new Android.Graphics.Color(ContextCompat.GetColor(Context, Resource.Color.bottomSheetHeaderButtonColor));
			button.SetTextColor(color);
		}

		button.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
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

        VirtualView.OnCompleteOpenCloseAction(true);
    }

	public void DismissBottomSheet()
	{
		if (bottomSheetDialog is not null)
		{
			bottomSheetDialog.DismissEvent -= BottomSheetDialog_DismissEvent;
			bottomSheetDialog.Dismiss();

            VirtualView.OnCompleteOpenCloseAction(false);
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


public class BottomSheetLayoutChangeListener : Java.Lang.Object, AndroidView.IOnLayoutChangeListener
{
	private readonly MauiBottomSheet bottomSheet;

	public BottomSheetLayoutChangeListener(MauiBottomSheet mauiBottomSheet) : base()
	{
		bottomSheet = mauiBottomSheet;
	}

    public void OnLayoutChange(
		AndroidView? v, 
		int left, 
		int top, 
		int right, 
		int bottom, 
		int oldLeft, 
		int oldTop, 
		int oldRight, 
		int oldBottom)
    {
		bottomSheet.SetPeek();
    }
}

