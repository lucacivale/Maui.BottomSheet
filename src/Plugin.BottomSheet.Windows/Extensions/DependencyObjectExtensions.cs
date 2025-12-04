using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Plugin.BottomSheet.Windows.Extensions;

#pragma warning disable SA1600 // Elements should be documented
internal static class DependencyObjectExtensions
{
    internal static FrameworkElement? FindDescendant(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
    {
        PredicateByName predicateByName = new(name, comparisonType);

        return FindDescendant<FrameworkElement, PredicateByName>(element, ref predicateByName);
    }

    internal static T? FindAscendant<T>(this DependencyObject element)
#if HAS_UNO
		where T : class, DependencyObject
#else
    where T : notnull, DependencyObject
#endif
    {
        PredicateByAny<T> predicateByAny = default;

        return FindAscendant<T, PredicateByAny<T>>(element, ref predicateByAny);
    }

    internal static T? FindAscendant<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
#if HAS_UNO
		where T : class, DependencyObject
#else
    where T : notnull, DependencyObject
#endif
    {
        PredicateByFunc<T, TState> predicateByFunc = new(state, predicate);

        return FindAscendant<T, PredicateByFunc<T, TState>>(element, ref predicateByFunc);
    }

    private static T? FindDescendant<T, TPredicate>(this DependencyObject element, ref TPredicate predicate)
#if HAS_UNO
        where T : class, DependencyObject
#else
    where T : notnull, DependencyObject
#endif
    where TPredicate : struct, IPredicate<T>
    {
        int childrenCount = VisualTreeHelper.GetChildrenCount(element);

        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(element, i);

            if (child is T result && predicate.Match(result))
            {
                return result;
            }

            T? descendant = FindDescendant<T, TPredicate>(child, ref predicate);

            if (descendant is not null)
            {
                return descendant;
            }
        }

        return null;
    }

    private static T? FindAscendant<T, TPredicate>(this DependencyObject element, ref TPredicate predicate)
#if HAS_UNO
		where T : class, DependencyObject
#else
        where T : notnull, DependencyObject
#endif
        where TPredicate : struct, IPredicate<T>
    {
        while (true)
        {
            DependencyObject? parent = VisualTreeHelper.GetParent(element);

            if (parent is null)
            {
                return null;
            }

            if (parent is T result && predicate.Match(result))
            {
                return result;
            }

            element = parent;
        }
    }
}
#pragma warning restore SA1600 // Elements should be documented
