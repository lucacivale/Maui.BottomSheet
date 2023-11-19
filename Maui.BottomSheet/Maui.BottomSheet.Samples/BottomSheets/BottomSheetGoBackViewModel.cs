using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.BottomSheet;
using Maui.BottomSheet.Navigation;

public partial class BottomSheetGoBackViewModel : ObservableObject
{
	private IBottomSheetNavigationService _navigationService;

	public BottomSheetGoBackViewModel(IBottomSheetNavigationService bottomSheetNavigationService)
	{
		_navigationService = bottomSheetNavigationService;
	}

    [RelayCommand]
    private void GoBackSetCallerSheetState()
    {
        IBottomSheetNavigationParameters parameters = new BottomSheetNavigationParameters()
		{
			{ nameof(IBottomSheet.SelectedSheetState), BottomSheetState.Medium },
		};

        _navigationService.GoBack(parameters);
    }

    [RelayCommand]
    private void ClearSheetStack()
    {
        _navigationService.ClearBottomSheetStack();
    }
}