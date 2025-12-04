using System.Runtime.CompilerServices;

namespace Plugin.BottomSheet.Windows.Extensions;

#pragma warning disable SA1600 // Elements should be documented
internal readonly struct PredicateByAny<T> : IPredicate<T>
    where T : class
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Match(T element)
    {
        return true;
    }
}
#pragma warning restore SA1600 // Elements should be documented
