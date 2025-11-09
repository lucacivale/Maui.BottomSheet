namespace Plugin.Maui.BottomSheet;

[ContentProperty(nameof(Name))]
public sealed class PeekViewExtension : IMarkupExtension<double>
{
    private WeakReference<BottomSheet>? _bottomSheet;
    private WeakReference<View>? _view;

    public string? Name { get; set; }

    public View? View { get; set; }

    public double ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget provideValueTarget = serviceProvider.GetRequiredService<IProvideValueTarget>();

        _bottomSheet = new((BottomSheet)provideValueTarget.TargetObject);

        if (_bottomSheet.TryGetTarget(out var sheet))
        {
            sheet.Opened += SheetOnOpened;
            sheet.Closing += SheetOnClosing;
        }

        return 0;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }

    private void SheetOnOpened(object? sender, EventArgs e)
    {
        if (sender is not BottomSheet bottomSheet)
        {
            return;
        }

        if (View is null)
        {
            if (string.IsNullOrWhiteSpace(Name) == false
                && bottomSheet.Content?.Content is not null)
            {
                View view = bottomSheet.Content.Content.FindByName<View>(Name);
                view.Loaded += OnLoaded;

                _view = new(view);
            }
        }
        else
        {
            View.Loaded += OnLoaded;

            _view = new(View);
        }
    }

    private void SheetOnClosing(object? sender, EventArgs e)
    {
        if (_view?.TryGetTarget(out var view) == true)
        {
            view.Loaded -= OnLoaded;
            view.MeasureInvalidated -= ContentOnMeasureInvalidated;
        }
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        if (sender is not View view)
        {
            return;
        }

        SetPeekHeight(view);

        view.MeasureInvalidated += ContentOnMeasureInvalidated;
    }

    private void ContentOnMeasureInvalidated(object? sender, EventArgs e)
    {
        if (sender is not View view)
        {
            return;
        }

        SetPeekHeight(view);
    }

    private void SetPeekHeight(View view)
    {
        if (_bottomSheet?.TryGetTarget(out var bottomSheet) == true)
        {
            if (view is ICrossPlatformLayout crossPlatformLayout)
            {
                bottomSheet.PeekHeight = crossPlatformLayout.CrossPlatformMeasure(view.Window?.Width ?? double.PositiveInfinity, view.Window?.Height ?? double.NegativeInfinity).Height;
            }
            else
            {
                bottomSheet.PeekHeight = view.Height;

                if (bottomSheet.PeekHeight <= 0)
                {
                    bottomSheet.PeekHeight = view.Measure(view.Window?.Width ?? double.PositiveInfinity, view.Window?.Height ?? double.NegativeInfinity).Height;
                }
            }
        }
    }
}