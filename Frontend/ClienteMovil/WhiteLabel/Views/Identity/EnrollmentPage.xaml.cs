using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLabel.Models;
using WhiteLabel.ViewModels.Identity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhiteLabel.Views.Identity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnrollmentPage : ContentPage
    {
        public EnrollmentPage()
        {
            InitializeComponent();
        }

        public EnrollmentPage(ProfileData profile) : this()
        {
            BindingContext = new EnrollmentViewModel(Navigation);
            ((EnrollmentViewModel)BindingContext).Profile = profile;
        }
    }
}