using System.Runtime.CompilerServices;

namespace Plugin.BottomSheet.Windows.Extensions;

#pragma warning disable SA1600 // Elements should be documented
internal readonly struct PredicateByFunc<T, TState> : IPredicate<T>
    where T : class
{
    private readonly TState _state;
    private readonly Func<T, TState, bool> _predicate;

    public PredicateByFunc(TState state, Func<T, TState, bool> predicate)
    {
        _state = state;
        _predicate = predicate;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Match(T element)
    {
        return this._predicate(element, _state);
    }
}
#pragma warning restore SA1600 // Elements should be documented
