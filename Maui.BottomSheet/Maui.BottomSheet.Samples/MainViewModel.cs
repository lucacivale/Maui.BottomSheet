using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.BottomSheet.Samples.Services;

namespace Maui.BottomSheet.Samples;

public partial class MainViewModel : ObservableObject
{
	private readonly IDialogService _dialogService;

	public MainViewModel(IDialogService dialogService)
	{
		_dialogService = dialogService;
	}

	#region Base Sheet

	[ObservableProperty]
	private bool isBaseSheetOpen;

	[ObservableProperty]
	private BottomSheetState baseStates;

	[RelayCommand]
	private void OpenBaseSheet()
	{
		IsBaseSheetOpen = true;
	}

	[RelayCommand]
	private void SetMediumState()
	{
		BaseStates = BottomSheetState.Medium;
	}
	[RelayCommand]
	private void SetLargeState()
	{
		BaseStates = BottomSheetState.Large;
	}
	[RelayCommand]
	private void SetMediumAndLargeState()
	{
		BaseStates = BottomSheetState.All;
	}
	#endregion

	#region Simple Header Sheet
	[RelayCommand]
	private void OpenSimpleHeaderSheet()
	{
		IsSimpleHeaderSheetOpen = true;
	}

	[ObservableProperty]
	private bool hasHandle = true;
	
	[ObservableProperty]
	private bool isSimpleHeaderSheetOpen;

	[ObservableProperty]
	private bool showSimpleHeaderSheetHeader = true;

	[ObservableProperty]
	private bool showSimpleTitleSheetHeader = true;

	partial void OnShowSimpleTitleSheetHeaderChanged(bool value)
	{
		if (value == false)
		{
			SimpleTitle = string.Empty;
		}
	}

	[ObservableProperty]
	private BottomSheetHeaderAppearanceMode simpleHeaderAppearanceMode = BottomSheetHeaderAppearanceMode.LeftAndRightButton;

	[ObservableProperty]
	private BottomSheetState selectedState;

	[ObservableProperty]
	private string simpleTitle = "Simple";

	[ObservableProperty]
	private string simpleTopRightButtonText = "Large";

	[ObservableProperty]
	private string simpleTopLeftButtonText = "Medium";

	[RelayCommand]
	private void SimpleTopRight()
	{
		SelectedState = BottomSheetState.Large;
	}

	[RelayCommand]
	private void SimpleTopLeft()
	{
		SelectedState = BottomSheetState.Medium;
	}

	[RelayCommand]
	private void SimpleHeaderAppearanceNone()
	{
		SimpleHeaderAppearanceMode = BottomSheetHeaderAppearanceMode.None;
	}

	[RelayCommand]
	private void SimpleHeaderAppearanceLeft()
	{
		SimpleHeaderAppearanceMode = BottomSheetHeaderAppearanceMode.LeftButton;
	}

	[RelayCommand]
	private void SimpleHeaderAppearanceRight()
	{
		SimpleHeaderAppearanceMode = BottomSheetHeaderAppearanceMode.RightButton;
	}

	[RelayCommand]
	private void SimpleHeaderAppearanceBoth()
	{
		SimpleHeaderAppearanceMode = BottomSheetHeaderAppearanceMode.LeftAndRightButton;
	}
	#endregion

	#region Safe Area
	[ObservableProperty]
	private bool isSafeAreaSheetOpen;

	[ObservableProperty]
	private bool ignoresafearea;

	[RelayCommand]
	private void OpenSafeAreaSheet()
	{
		IsSafeAreaSheetOpen = true;
	}
	#endregion

	#region Custom header sheet
	[ObservableProperty]
	private bool isCustomHeaderOpen;

	[RelayCommand]
	private void OpenCustomHeader()
	{
		IsCustomHeaderOpen = true;
	}

	[RelayCommand]
	private async Task OpenedCustomHeader()
	{
		await _dialogService.DisplayAlert("Info", "Opened", "Cancel");
	}

	[RelayCommand]
	private async Task ClosingCustomHeader()
	{
		await _dialogService.DisplayAlert("Info", "Closing", "Cancel");
	}
	#endregion
}

