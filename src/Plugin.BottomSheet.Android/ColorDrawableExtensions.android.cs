namespace Plugin.BottomSheet.Android;

/// <summary>
/// Extension methods for animating color changes in ColorDrawable objects.
/// </summary>
internal static class ColorDrawableExtensions
{
    /// <summary>
    /// Animates a color change on a ColorDrawable from one ARGB value to another.
    /// </summary>
    /// <param name="drawable">The ColorDrawable to animate.</param>
    /// <param name="from">The starting ARGB color value.</param>
    /// <param name="to">The ending ARGB color value.</param>
    /// <param name="duration">The animation duration in milliseconds. Default is 250 ms.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task AnimateChangeAsync(this ColorDrawable drawable, int from, int to, int duration = 250)
    {
        using ArgbEvaluator evaluator = new();
        using ValueAnimator? colorAnimation = ValueAnimator.OfObject(evaluator, from, to);

        if (colorAnimation is not null)
        {
            //todo kann weg oder?
            using Animator animator = colorAnimation.SetDuration(duration);

            TaskCompletionSource taskCompletionSource = new();
            using AnimationListener animationListener = new(taskCompletionSource);
            using AnimationUpdateListener animationUpdateListener = new(drawable);

            colorAnimation.AddListener(animationListener);
            colorAnimation.AddUpdateListener(animationUpdateListener);
            colorAnimation.Start();

            await taskCompletionSource.Task.ConfigureAwait(true);
            colorAnimation.RemoveAllListeners();
        }
    }
}