namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Access <see cref="BindingContext"/> of <see cref="BindableObject"/>.
/// </summary>
public interface IBindable
{
    /// <summary>
    /// Gets or sets an object that contains the properties that will be targeted by the bound properties that belong to this <see cref="BindableObject" />.
    /// This is a bindable property.
    /// </summary>
    object BindingContext { get; set; }
}