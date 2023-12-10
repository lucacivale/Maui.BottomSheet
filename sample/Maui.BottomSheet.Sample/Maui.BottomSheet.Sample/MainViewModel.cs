using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.BottomSheet.Navigation;
using Maui.BottomSheet.Samples.BottomSheets;
using Maui.BottomSheet.Samples.Services;
using Maui.BottomSheet.SheetBuilder;

namespace Maui.BottomSheet.Samples;

public partial class MainViewModel : ObservableObject
{
	private readonly IDialogService _dialogService;
	private readonly IBottomSheetNavigationService _bottomSheetNavigationService;
	private readonly IBottomSheetBuilderFactory _bottomSheetBuilderFactory;

	public MainViewModel(IDialogService dialogService, IBottomSheetNavigationService bottomSheetNavigationService, IBottomSheetBuilderFactory bottomSheetBuilderFactory)
	{
		_dialogService = dialogService;
		_bottomSheetNavigationService = bottomSheetNavigationService;
		_bottomSheetBuilderFactory = bottomSheetBuilderFactory;
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
    private bool isCancelable = false;

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

	#region Navigation
	[RelayCommand]
	private void NavigateToBottomSheetNoVM()
	{
		_bottomSheetNavigationService.NavigateTo<BottomSheetNoVM>();
	}

	[RelayCommand]
	private void NavigateToBottomSheetVM()
	{
		_bottomSheetNavigationService.NavigateTo<BottomSheetVM, BottomSheetVMViewModel>();
	}

	[RelayCommand]
	private void NavigateToBottomSheetVMWithParameters()
	{
		IBottomSheetNavigationParameters parameters = new BottomSheetNavigationParameters()
		{
			{ nameof(IBottomSheet.SelectedSheetState), BottomSheetState.Large },
		};

		_bottomSheetNavigationService.NavigateTo<BottomSheetVM, BottomSheetVMViewModel>(parameters);
	}

    [RelayCommand]
    private void OpenContentPageAsBottomSheet()
	{
		_bottomSheetBuilderFactory.Create()
			.FromContentPage<NewPageA>()
			.ConfigureBottomSheet((sheet) =>
			{
				sheet.SheetStates = BottomSheetState.All;
			})
			.WireTo<NewPageAViewModel>()
			.Open();
	}

    [RelayCommand]
    private void OpenViewAsBottomSheet()
    {
        _bottomSheetBuilderFactory.Create()
            .FromView<ContentA>()
            .ConfigureBottomSheet((sheet) =>
            {
                sheet.SheetStates = BottomSheetState.Medium;
            })
            .WireTo<ContentAViewModel>()
            .Open();
    }
    #endregion
}

