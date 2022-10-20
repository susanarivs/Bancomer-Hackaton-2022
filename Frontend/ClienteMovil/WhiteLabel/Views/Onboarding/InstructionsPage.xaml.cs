using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLabel.ViewModels.Onboarding;
using WhiteLabel.Views.Onboarding.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhiteLabel.Views.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InstructionsPage : WalkthroughBasePage
    {
        public InstructionsPage()
        {
            InitializeComponent();

            //Vista-Modelo
            BindingContext = new WalkthroughViewModel(Close, MoveNext);
        }
    }
}