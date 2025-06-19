using AArgbEvaluator= Android.Animation.ArgbEvaluator;
using AColor = Android.Graphics.Color;
using AColorDrawable = Android.Graphics.Drawables.ColorDrawable;
using AValueAnimator= Android.Animation.ValueAnimator;

namespace Plugin.Maui.BottomSheet.Platform.Android;

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
    public static void AnimateChange(this AColorDrawable drawable, int from, int to, int duration = 250)
    {
        using AArgbEvaluator evaluator = new();
        using AValueAnimator? colorAnimation = AValueAnimator.OfObject(evaluator, from, to);

        if (colorAnimation is not null)
        {
            colorAnimation.SetDuration(duration);
            colorAnimation.Update += (_, args) =>
            {
                if (args.Animation.AnimatedValue is not null)
                {
                    drawable.Color = AColor.Argb(
                        AColor.GetAlphaComponent((int)args.Animation.AnimatedValue),
                        AColor.GetRedComponent((int)args.Animation.AnimatedValue),
                        AColor.GetGreenComponent((int)args.Animation.AnimatedValue),
                        AColor.GetBlueComponent((int)args.Animation.AnimatedValue));
                }
            };
            colorAnimation.Start();
        }
    }
}