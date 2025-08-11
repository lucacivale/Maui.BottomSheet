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
    public static void AnimateChange(this ColorDrawable drawable, int from, int to, int duration = 250)
    {
        using ArgbEvaluator evaluator = new();
        using ValueAnimator? colorAnimation = ValueAnimator.OfObject(evaluator, from, to);

        if (colorAnimation is not null)
        {
            colorAnimation.SetDuration(duration);
            colorAnimation.Update += (_, args) =>
            {
                if (args.Animation.AnimatedValue is not null)
                {
                    drawable.Color = Color.Argb(
                        Color.GetAlphaComponent((int)args.Animation.AnimatedValue),
                        Color.GetRedComponent((int)args.Animation.AnimatedValue),
                        Color.GetGreenComponent((int)args.Animation.AnimatedValue),
                        Color.GetBlueComponent((int)args.Animation.AnimatedValue));
                }
            };
            colorAnimation.Start();
        }
    }
}