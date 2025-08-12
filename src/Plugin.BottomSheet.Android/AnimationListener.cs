namespace Plugin.BottomSheet.Android;

internal sealed class AnimationListener : Java.Lang.Object, Animator.IAnimatorListener
{
    private readonly TaskCompletionSource _taskCompletionSource;

    public AnimationListener(TaskCompletionSource taskCompletionSource)
    {
        _taskCompletionSource = taskCompletionSource;
    }

    public void OnAnimationCancel(Animator animation)
    {
        _ = _taskCompletionSource.TrySetCanceled();
    }

    public void OnAnimationEnd(Animator animation)
    {
        _ = _taskCompletionSource.TrySetResult();
    }

    public void OnAnimationRepeat(Animator animation)
    {
    }

    public void OnAnimationStart(Animator animation)
    {
    }
}
