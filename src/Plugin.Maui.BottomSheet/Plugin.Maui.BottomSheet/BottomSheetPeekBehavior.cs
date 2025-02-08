namespace Plugin.Maui.BottomSheet;

/// <inheritdoc />
public sealed class BottomSheetPeekBehavior : PlatformBehavior<View>
{
    #if ANDROID || IOS || MACCATALYST
    private WeakReference<IBottomSheet>? _weakBottomSheet;
    #endif

    #if ANDROID
    /// <inheritdoc/>
    protected override void OnAttachedTo(View bindable, Android.Views.View platformView)
    {
        base.OnAttachedTo(bindable, platformView);

#pragma warning disable CA1062
        bindable.SizeChanged += ViewOnSizeChanged;
#pragma warning restore CA1062
        var bottomSheet = bindable.FindBottomSheet();

        if (bottomSheet is not null)
        {
            _weakBottomSheet = new WeakReference<IBottomSheet>(bottomSheet);
        }
    }

    /// <inheritdoc/>
    protected override void OnDetachedFrom(View bindable, Android.Views.View platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

#pragma warning disable CA1062
        bindable.SizeChanged -= ViewOnSizeChanged;
#pragma warning restore CA1062
    }

    #elif MACCATALYST || IOS
    /// <inheritdoc/>
    protected override void OnAttachedTo(View bindable, UIKit.UIView platformView)
    {
        base.OnAttachedTo(bindable, platformView);

        double height = 0.00;
#pragma warning disable CA1062
        #if NET9
        var size = bindable.Measure(double.PositiveInfinity, double.PositiveInfinity);
        height = size.Height;
        #elif NET8
        var size = bindable.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);
        height = size.Request.Height;
        #endif
        bindable.SizeChanged += ViewOnSizeChanged;
#pragma warning restore CA1062
        var bottomSheetPage = bindable.FindBottomSheetPage();

        if (bottomSheetPage?.BottomSheet is not null)
        {
            _weakBottomSheet = new WeakReference<IBottomSheet>(bottomSheetPage.BottomSheet);
            bottomSheetPage.BottomSheet.PeekHeight = height;
        }
    }

    /// <inheritdoc/>
    protected override void OnDetachedFrom(View bindable, UIKit.UIView platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

#pragma warning disable CA1062
        bindable.SizeChanged -= ViewOnSizeChanged;
#pragma warning restore CA1062
    }
    #endif

    #if ANDROID || IOS || MACCATALYST
    private void ViewOnSizeChanged(object? sender, EventArgs e)
    {
        if (sender is View view
            && _weakBottomSheet?.TryGetTarget(out var bottomSheet) == true)
        {
            bottomSheet.PeekHeight = view.Height;
        }
    }
    #endif
}