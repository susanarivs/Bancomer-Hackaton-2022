using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WhiteLabel.Models;
using WhiteLabel.Services.Facetec;
using Xamarin.Forms;

namespace WhiteLabel.ViewModels.Identity
{
    public class EnrollmentViewModel : ObservableObject
    {
        private readonly INavigation _navigation;
        private readonly Command _signCommand;
        private bool _signing;
        private ProfileData _profileData;

        public EnrollmentViewModel(INavigation navigation) : base(listenCultureChanges: false)
        {
            _navigation = navigation;

            _signCommand = new Command(SignAction, () => !Signing);
        }

        public bool Signing
        {
            get { return _signing; }
            set
            {
                if (_signing != value)
                {
                    _signing = value;
                    _signCommand.ChangeCanExecute();
                }
            }
        }

        public ProfileData Profile
        {
            get { return _profileData; }
            set
            {
                if (_profileData != value)
                {
                    _profileData = value;
                    NotifyPropertyChanged("Profile");
                }
            }
        }

        public ICommand SignCommand => _signCommand;

        private /*async*/ void SignAction()
        {
            if (!Signing)
            {
                Signing = true;

                try
                {
                    //var result = await FacetecService.Instance.EnrollAsync();
                    //if (result)
                    //{
                    //    await _navigation.PopAsync();
                    //}
                }
                finally
                {
                    Signing = false;
                }
            }
        }
    }
}
