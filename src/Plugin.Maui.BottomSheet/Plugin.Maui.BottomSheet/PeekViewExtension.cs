namespace Plugin.Maui.BottomSheet;

[ContentProperty(nameof(Name))]
[RequireService([typeof(IProvideValueTarget)])]
public sealed class PeekViewExtension : IMarkupExtension<double>
{
    private WeakReference<BottomSheet>? _bottomSheet;
    private WeakReference<View>? _view;

    public string? Name { get; set; }

    public View? View { get; set; }

    public double ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget provideValueTarget = serviceProvider.GetRequiredService<IProvideValueTarget>();

        BottomSheet sheet = (BottomSheet)provideValueTarget.TargetObject;
        sheet.Loaded += SheetOnLoaded;
        sheet.Unloaded += SheetOnUnloaded;

        _bottomSheet = new(sheet);

        return 0;
    }

    private void SheetOnLoaded(object? sender, EventArgs e)
    {
        if (_bottomSheet?.TryGetTarget(out BottomSheet? sheet) == true)
        {
            sheet.Opening += SheetOnOpening;
            sheet.Closing += SheetOnClosing;
        }
    }

    private void SheetOnUnloaded(object? sender, EventArgs e)
    {
        if (_bottomSheet?.TryGetTarget(out BottomSheet? sheet) == true)
        {
            sheet.Opening -= SheetOnOpening;
            sheet.Closing -= SheetOnClosing;
        }
    }

    private void SheetOnOpening(object? sender, EventArgs e)
    {
        if (_bottomSheet?.TryGetTarget(out BottomSheet? sheet) == true)
        {
            sheet.LayoutChanged += SheetOnLayoutChanged;
        }
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }

    private void SheetOnClosing(object? sender, EventArgs e)
    {
        if (_bottomSheet?.TryGetTarget(out BottomSheet? sheet) == true)
        {
            sheet.LayoutChanged -= SheetOnLayoutChanged;
        }
    }

    private void SheetOnLayoutChanged(object? sender, EventArgs e)
    {
        if (_view is null)
        {
            if (View is null)
            {
                if (string.IsNullOrWhiteSpace(Name) == false
                    && _bottomSheet?.TryGetTarget(out BottomSheet? bottomSheet) == true
                    && bottomSheet.Content?.Content is not null)
                {
                    _view = new(bottomSheet.Content.Content.FindByName<View>(Name));
                }
            }
            else
            {
                _view = new(View);
            }
        }

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