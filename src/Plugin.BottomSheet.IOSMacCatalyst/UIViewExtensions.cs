namespace Plugin.BottomSheet.IOSMacCatalyst;

// ReSharper disable once InconsistentNaming
internal static class UIViewExtensions
{
    public static void HeightConstraint(this UIView view, double height)
    {
        NSLayoutConstraint constraint = view.Constraints.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Height, view.HeightAnchor.ConstraintEqualTo(new nfloat(30)));

        constraint.Constant = new nfloat(height);
        constraint.Active = true;
    }
}