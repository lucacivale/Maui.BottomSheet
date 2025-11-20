namespace Plugin.BottomSheet.IOSMacCatalyst;

/// <inheritdoc />
internal sealed class BottomSheetContainerViewController : UIViewController
{
    private readonly UIView? _content;

    public BottomSheetContainerViewController(UIView content)
    {
        _content = content;
    }

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

    public override void RemoveFromParentViewController()
    {
        base.RemoveFromParentViewController();

        _content?.RemoveFromSuperview();
    }
}