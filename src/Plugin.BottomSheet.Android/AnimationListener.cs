namespace Plugin.BottomSheet.Android;

internal sealed class AnimationListener : Java.Lang.Object, Animator.IAnimatorListener, ValueAnimator.IAnimatorUpdateListener
{
    private readonly TaskCompletionSource _taskCompletionSource;
    private readonly WeakReference<ColorDrawable> _weakDrawable;

    private bool _animationEnded;

    public AnimationListener(ColorDrawable drawable, TaskCompletionSource taskCompletionSource)
    {
        _weakDrawable = new(drawable);
        _taskCompletionSource = taskCompletionSource;
    }

    public void OnAnimationCancel(Animator animation)
    {
        _animationEnded = true;
        _ = _taskCompletionSource.TrySetCanceled();
    }

    public void OnAnimationEnd(Animator animation)
    {
        _animationEnded = true;
        _ = _taskCompletionSource.TrySetResult();
    }

    public void OnAnimationRepeat(Animator animation)
    {
    }

    public void OnAnimationStart(Animator animation)
    {
    }

    public void OnAnimationUpdate(ValueAnimator animation)
    {
        if (_animationEnded == false
            && animation.AnimatedValue is not null
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
