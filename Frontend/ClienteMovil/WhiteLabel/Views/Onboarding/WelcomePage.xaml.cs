using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLabel.ViewModels.Onboarding;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhiteLabel.Views.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();

            BindingContext = new WelcomeViewModel(Navigation);
        }
    }
}