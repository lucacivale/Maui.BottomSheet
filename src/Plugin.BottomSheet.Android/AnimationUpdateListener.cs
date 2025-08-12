namespace Plugin.BottomSheet.Android;

internal sealed class AnimationUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
{
    private readonly WeakReference<ColorDrawable> _weakDrawable;

    public AnimationUpdateListener(ColorDrawable drawable)
    {
        _weakDrawable = new(drawable);
    }

    public void OnAnimationUpdate(ValueAnimator animation)
    {
        if (animation.AnimatedValue is not null
            && _weakDrawable.TryGetTarget(out ColorDrawable? drawable))
        {
            drawable.Color = Color.Argb(
                Color.GetAlphaComponent((int)animation.AnimatedValue),
                Color.GetRedComponent((int)animation.AnimatedValue),
                Color.GetGreenComponent((int)animation.AnimatedValue),
                Color.GetBlueComponent((int)animation.AnimatedValue));
        }
    }
}
