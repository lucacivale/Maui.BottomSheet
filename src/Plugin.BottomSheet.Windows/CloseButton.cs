using Microsoft.UI.Xaml.Controls;

namespace Plugin.BottomSheet.Windows;

internal sealed partial class CloseButton : Button
{
    public CloseButton()
    {
        Content = new SymbolIcon(Symbol.Cancel);
    }

    public void SetHeight(double height)
    {
        MinHeight = height;

        SetIconSize();
    }

    public void SetWidth(double width)
    {
        MinWidth = width;

        SetIconSize();
    }

    private void SetIconSize()
    {
        FontSize = Convert.ToInt32((MinHeight + MinWidth) / 2);
    }
}
