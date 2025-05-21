using AArgbEvaluator= Android.Animation.ArgbEvaluator;
using AColor = Android.Graphics.Color;
using AColorDrawable = Android.Graphics.Drawables.ColorDrawable;
using AValueAnimator= Android.Animation.ValueAnimator;

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// Color drawable extension methods.
/// </summary>
internal static class ColorDrawableExtensions
{
    /// <summary>
    /// Animate color change.
    /// </summary>
    /// <param name="drawable">Drawable to change.</param>
    /// <param name="from">From color as argb value.</param>
    /// <param name="to">To color as argb value.</param>
    /// <param name="duration">Animation duration in ms.</param>
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