using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using WhiteLabel.Loading;
using WhiteLabel.Services.Facetec;
using WhiteLabel.Views.Identity;
using WhiteLabel.Views.Onboarding;
using Xamarin.Forms;

namespace WhiteLabel.ViewModels.Identity
{
    public class OtpVerificationViewModel : ObservableObject
    {
        private readonly INavigation _navigation;
        private readonly IZoomSdk _zoomPlatform;

        private readonly Command _startEnrollCommand;
        private bool _startingEnroll;

        public OtpVerificationViewModel(INavigation navigation) : base(listenCultureChanges: false)
        {
            _navigation = navigation;

            _startEnrollCommand = new Command(StartEnrollAction, () => !StartingEnroll);

            IZoomFactory zoomFactory = DependencyService.Get<IZoomFactory>();
            if (zoomFactory != null)
            {
                _zoomPlatform = zoomFactory.CreateZoomScanner();

                //DependencyService.Get<ILodingPageService>().ShowLoadingPage();
                MessagingCenter.Subscribe<MessagesZoom.ScannerStatusMessage>(this, MessagesZoom.ScannerSetup, (sender) =>
                {
                    var ready = sender.ScannerReady;
                    //DependencyService.Get<ILodingPageService>().HideLoadingPage();
                });
            }
        }

        public bool StartingEnroll
        {
            get { return _startingEnroll; }
            set
            {
                if (_startingEnroll != value)
                {
                    _startingEnroll = value;
                    _startEnrollCommand.ChangeCanExecute();
                }
            }
        }

        public ICommand StartEnrollCommand => _startEnrollCommand;

        private void StartEnrollAction()
        {
            if (!StartingEnroll)
            {
                StartingEnroll = true;

                try
                {
                    if (_zoomPlatform != null)
                    {
                        _zoomPlatform.Scan();
                        MessagingCenter.Subscribe<MessagesZoom.ScanningDoneMessage>(this, MessagesZoom.ScanningDoneMessageId, (sender) =>
                        {
                            string stringResult = "Sin resultados válidos.";
                            if (sender.ScanningCancelled)
                            {
                                stringResult = "Escaneo cancelado";
                                //mostrar una pantalla de error
                            }
                            else
                            {
                                stringResult = "Escaneo valido";
                                //mostrar la siguiente pantalla de usuario con contrato cargado
                                _navigation.PushAsync(new EnrollmentPage());
                            }
                        });
                    }
                }
                finally
                {
                    StartingEnroll = false;
                }
            }
        }
    }
}
