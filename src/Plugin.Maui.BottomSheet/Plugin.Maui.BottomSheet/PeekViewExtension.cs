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
                _view = new(bottomSheet.Content.Content.FindByName<View>(Name));
            }
        }
        else
        {
            _view = new(View);
        }

        bottomSheet.ContainerView.Loaded += OnLoaded;
    }

    private void SheetOnClosing(object? sender, EventArgs e)
    {
        if (_bottomSheet?.TryGetTarget(out var view) == true)
        {
            view.ContainerView.Loaded -= OnLoaded;
            view.ContainerView.MeasureInvalidated -= ContentOnMeasureInvalidated;
        }

        if (_bottomSheet?.TryGetTarget(out BottomSheet? bottomSheet) == true)
        {
            bottomSheet.MeasureInvalidated -= ContentOnMeasureInvalidated;
        }
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        if (sender is not View view)
        {
            return;
        }

        SetPeekHeight();

        view.MeasureInvalidated += ContentOnMeasureInvalidated;

        if (_bottomSheet?.TryGetTarget(out BottomSheet? bottomSheet) == true)
        {
            bottomSheet.MeasureInvalidated += ContentOnMeasureInvalidated;
        }
    }

    private void ContentOnMeasureInvalidated(object? sender, EventArgs e)
    {
        SetPeekHeight();
    }

    private void SetPeekHeight()
    {
        if (_bottomSheet?.TryGetTarget(out BottomSheet? bottomSheet) == true)
        {
            double peekHeight = 0;

            if (_view?.TryGetTarget(out View? view) == true)
            {
                peekHeight += view.Measure().Height;
            }

            if (bottomSheet.ContainerView.TryGetView(BottomSheet.HandleRow, out View? handle))
            {
                peekHeight += handle.Measure().Height;
            }

            if (bottomSheet.ContainerView.TryGetView(BottomSheet.HeaderRow, out View? header))
            {
                peekHeight += header.Measure().Height;
            }

            bottomSheet.PeekHeight = peekHeight;
        }
    }
}