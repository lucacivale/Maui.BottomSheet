using Microsoft.UI.Xaml;
using System.Runtime.CompilerServices;

namespace Plugin.BottomSheet.Windows.Extensions;

#pragma warning disable SA1600 // Elements should be documented
internal readonly struct PredicateByName : IPredicate<FrameworkElement>
{
    private readonly string _name;
    private readonly StringComparison _comparisonType;

    public PredicateByName(string name, StringComparison comparisonType)
    {
        _name = name;
        _comparisonType = comparisonType;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Match(FrameworkElement element)
    {
        return element.Name?.Equals(_name, _comparisonType) ?? false;
    }
}
#pragma warning restore SA1600 // Elements should be documented
