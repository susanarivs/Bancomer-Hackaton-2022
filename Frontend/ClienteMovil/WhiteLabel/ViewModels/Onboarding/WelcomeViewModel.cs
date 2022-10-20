using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WhiteLabel.Views.Onboarding;
using Xamarin.Forms;

namespace WhiteLabel.ViewModels.Onboarding
{
    public class WelcomeViewModel : ObservableObject
    {
        private readonly INavigation _navigation;
        private readonly Command _startCommand;
        private bool _starting;

        public WelcomeViewModel(INavigation navigation) : base(listenCultureChanges: false)
        {
            _navigation = navigation;

            _startCommand = new Command(StartAction, () => !Starting);
        }

        public bool Starting
        {
            get { return _starting; }
            set
            {
                if (_starting != value)
                {
                    _starting = value;
                    _startCommand.ChangeCanExecute();
                }
            }
        }

        public ICommand StartCommand => _startCommand;

        private void StartAction()
        {
            if (!Starting)
            {
                Starting = true;

                try
                {
                    _navigation.PushAsync(new InstructionsPage());
                }
                finally
                {
                    Starting = false;
                }
            }
        }
    }
}
