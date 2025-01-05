using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Maui.BottomSheet.Sample;

public partial class ShowCasePage : ContentPage
{
    public ShowCasePage(ShowCaseViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}