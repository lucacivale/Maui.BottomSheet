namespace Plugin.BottomSheet.iOSMacCatalyst;

/// <summary>
/// A view controller that serves as a container for managing a content view presented within a bottom sheet interface.
/// </summary>
/// <remarks>
/// The <c>BottomSheetContainerViewController</c> provides a mechanism to display a given <c>UIView</c> as its primary content.
/// The content view is constrained to the safe area of the parent view, and its lifecycle is tied to that of the controller.
/// </remarks>
internal sealed class BottomSheetContainerViewController : UIViewController
{
    private readonly UIView? _content;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetContainerViewController"/> class.
    /// Represents a view controller that acts as a container for a content view within a bottom sheet.
    /// </summary>
    /// <param name="content">The view to be displayed within the container.</param>
    public BottomSheetContainerViewController(UIView content)
    {
        _content = content;
    }

    /// <summary>
    /// Called after the controller's view has been loaded into memory.
    /// </summary>
    /// <remarks>
    /// This method initializes the view hierarchy by adding the content view as a subview
    /// and configuring its auto-layout constraints to align it with the safe area of the parent view.
    /// No action is taken if the view or content is null.
    /// </remarks>
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        if (View is null
            || _content is null)
        {
            return;
        }

        View.AddSubview(_content);

        _content.TranslatesAutoresizingMaskIntoConstraints = false;
        NSLayoutConstraint.ActivateConstraints(
        [
            _content.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor),
            _content.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor),
            _content.TrailingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TrailingAnchor),
            _content.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor),
        ]);
    }

    /// <summary>
    /// Removes the view controller from its parent, if attached, and performs cleanup specific to the
    /// associated content view. This method overrides the base implementation to additionally remove
    /// the content view from its parent view.
    /// </summary>
    public override void RemoveFromParentViewController()
    {
        base.RemoveFromParentViewController();

        _content?.RemoveFromSuperview();
    }
}