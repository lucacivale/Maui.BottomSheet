namespace Maui.BottomSheet;

public class BottomSheetPeek : BindableObject
{
	#region Bindable Properties
	public static readonly BindableProperty IgnoreSafeAreaProperty = BindableProperty.Create(
		nameof(IgnoreSafeArea),
		typeof(bool),
		typeof(BottomSheetPeek),
		defaultValue: false,
		defaultBindingMode: BindingMode.TwoWay);

	public static readonly BindableProperty PeekHeightProperty = BindableProperty.Create(
		nameof(PeekHeight),
		typeof(double),
		typeof(BottomSheetPeek),
		defaultValue: double.NaN,
		defaultBindingMode:BindingMode.TwoWay);
	#endregion

	#region Properties
	public bool IgnoreSafeArea { get => (bool)GetValue(IgnoreSafeAreaProperty); set => SetValue(IgnoreSafeAreaProperty, value); }
	public double PeekHeight { get => (double)GetValue(PeekHeightProperty); set => SetValue(PeekHeightProperty, value); }

	public DataTemplate? PeekViewDataTemplate { get; set; }
	#endregion
}

