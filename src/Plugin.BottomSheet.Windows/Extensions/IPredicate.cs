namespace Plugin.BottomSheet.Windows.Extensions;

#pragma warning disable SA1600 // Elements should be documented
internal interface IPredicate<in T>
    where T : class
{
    bool Match(T element);
}
#pragma warning restore SA1600 // Elements should be documented
