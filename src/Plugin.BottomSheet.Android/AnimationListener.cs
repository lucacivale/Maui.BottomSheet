using System.Diagnostics;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// A listener that handles animations for ColorDrawable objects and provides task-based notification of animation completion.
/// </summary>
internal sealed class AnimationListener : Java.Lang.Object, Animator.IAnimatorListener, ValueAnimator.IAnimatorUpdateListener
{
    private readonly TaskCompletionSource _taskCompletionSource;
    private readonly WeakReference<ColorDrawable> _weakDrawable;

    private bool _animationEnded;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationListener"/> class.
    /// Handles animation events and updates for a <see cref="ColorDrawable"/> object.
    /// </summary>
    /// <remarks>
    /// This class implements <see cref="Animator.IAnimatorListener"/> and <see cref="ValueAnimator.IAnimatorUpdateListener"/>
    /// to handle animation lifecycle events and update the color of a <see cref="ColorDrawable"/> during an animation.
    /// </remarks>
    /// <param name="drawable">The target drawable to update during animation.</param>
    /// <param name="taskCompletionSource">The task completion source to notify when the animation completes.</param>
    public AnimationListener(ColorDrawable drawable, TaskCompletionSource taskCompletionSource)
    {
        _weakDrawable = new(drawable);
        _taskCompletionSource = taskCompletionSource;
    }

    /// <summary>
    /// Called when the animation is canceled.
    /// </summary>
    /// <param name="animation">The animation that was canceled.</param>
    public void OnAnimationCancel(Animator animation)
    {
        _animationEnded = true;
        _ = _taskCompletionSource.TrySetCanceled();
    }

    /// <summary>
    /// Called when the animation ends.
    /// Marks the animation as complete and sets the task result to indicate completion.
    /// </summary>
    /// <param name="animation">The Animator instance associated with the ended animation.</param>
    public void OnAnimationEnd(Animator animation)
    {
        _animationEnded = true;
        _ = _taskCompletionSource.TrySetResult();
    }

    /// <summary>
    /// Called when the animation is repeated. This method is triggered for each repetition
    /// of the animation cycle.
    /// </summary>
    /// <param name="animation">
    /// The animator instance associated with this animation event.
    /// </param>
    public void OnAnimationRepeat(Animator animation)
    {
    }

    /// <summary>
    /// Invoked when the animation starts.
    /// </summary>
    /// <param name="animation">The animator instance that started.</param>
    public void OnAnimationStart(Animator animation)
    {
    }

    /// <summary>
    /// Called when the animation is updated. Updates the color of the target drawable
    /// based on the animated value received from the ValueAnimator.
    /// </summary>
    /// <param name="animation">The ValueAnimator providing the current animation state and values.</param>
    public void OnAnimationUpdate(ValueAnimator animation)
    {
        try
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
        catch (ObjectDisposedException ex)
        {
            Trace.TraceError("Window background animation failed: {0}", ex);
        }
    }
}
