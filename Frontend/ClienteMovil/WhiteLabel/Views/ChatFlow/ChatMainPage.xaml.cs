using Flurl.Http;
using Flurl.Http.Configuration;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WhiteLabel.Helpers;
using WhiteLabel.Loading;
using WhiteLabel.Models;
using WhiteLabel.Services.Database;
using WhiteLabel.Services.Messages;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhiteLabel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatMainPage : ContentPage
    {
        private readonly Repository<PersonData> _repositoryDocs = Repository<PersonData>.Instance();

        private IFlurlClient _flurlClient;
        private bool lockWasTaken;
        private bool offlineMode;
        private bool enableCompression;
        private const string address = "http://dt01.azurewebsites.net";
        private const string uploadImageUrl = "Ine/ExtraerDatos2";
        private const string ND = "No determinado";

        public ChatMainPage()
        {
            InitializeComponent();
            BindingContext = new ChatMainViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            contacts.SelectedItem = null;
            if (lockWasTaken == false)
            {
                ((ChatMainViewModel)BindingContext).ReloadData();
            }

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = e.NetworkAccess;
            switch (current)
            {
                case NetworkAccess.Internet:
                    offlineMode = false;
                    break;
                case NetworkAccess.Local:
                case NetworkAccess.ConstrainedInternet:
                case NetworkAccess.None:
                case NetworkAccess.Unknown:
                    offlineMode = true;
                    break;
            }

            var profiles = e.ConnectionProfiles;
            enableCompression = !profiles.Contains(ConnectionProfile.WiFi);
        }

        private async void OnContactTapped(object sender, ItemTappedEventArgs e)
        {
            if (((ListView)sender).SelectedItem is PersonDataView contact) await Navigation.PushAsync(new ContactDetailPage(contact.Id));
        }

        private async void OnAddContactClicked(object sender, EventArgs e)
        {
            if (offlineMode)
            {
                DependencyService.Get<IMessage>().ShortAlert("Debe estar conectado a internet.");
                return;
            }

            await ScanStartAsync();
        }

        private async void OnTakeGalleryClicked(object sender, EventArgs e)
        {
            if (offlineMode)
            {
                DependencyService.Get<IMessage>().ShortAlert("Debe estar conectado a internet.");
                return;
            }

            await ScanStartAsync(false);
        }

        private async Task ScanStartAsync(bool fromCamera = true)
        {
            await CrossMedia.Current.Initialize();
            var fileName = $"{Guid.NewGuid()}.jpg";

            MediaFile file = null;
            if (fromCamera)
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DependencyService.Get<IMessage>().ShortAlert("No hay camara disponible.");
                    return;
                }
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 1400,
                    CompressionQuality = enableCompression ? 65 : 85,  //0 a 100 siendo 100 sin compresión
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    Directory = "Documentos",
                    Name = fileName
                };

                file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
            }
            else
            {
                var pickMediaOptions = new Plugin.Media.Abstractions.PickMediaOptions
                {
                    CompressionQuality = enableCompression ? 65 : 85
                };
                file = await CrossMedia.Current.PickPhotoAsync(pickMediaOptions);
            }

            if (file == null)
            {
                await Task.CompletedTask;
                return;
            }

            DependencyService.Get<ILodingPageService>().ShowLoadingPage();
            var bytes = await File.ReadAllBytesAsync(file.Path);
            var base64 = Convert.ToBase64String(bytes);

            if (fromCamera)
            {
                if (File.Exists(file.Path))
                {
                    File.Delete(file.Path);
                }
            }

            //enviar al motor
            var validAddress = new Uri(address).IsWellFormedOriginalString();
            if (!validAddress)
            {
                DependencyService.Get<ILodingPageService>().HideLoadingPage();
                DependencyService.Get<IMessage>().ShortAlert("La dirección del servidor ocr no esta bien formada.");

                await Task.CompletedTask;
                return;
            }

            var factory = new PerBaseUrlFlurlClientFactory();
            _flurlClient = factory.Get(new Flurl.Url(address));
            _flurlClient.Settings.HttpClientFactory = new UntrustedCertClientFactory();

            var bodyRequest = new OcrRequest
            {
                ImagenBase64 = base64
            };

            var preOcr = await NewOcr(bodyRequest);
            if (preOcr == null)
            {
                DependencyService.Get<ILodingPageService>().HideLoadingPage();
                DependencyService.Get<IMessage>().ShortAlert("Ocurrió un error interno.");

                if (File.Exists(file.Path))
                {
                    File.Delete(file.Path);
                }

                await Task.CompletedTask;
                return;
            }

            var data = new PersonData();
            data.Name = preOcr.Nombres;
            data.ApellidoPaterno = preOcr.ApellidoPaterno;
            data.ApellidoMaterno = preOcr.ApellidoMaterno;
            data.Avatar = Constantes.avatarDefault;
            data.Address = preOcr.Domicilio;
            data.Curp = preOcr.Curp;
            data.Gender = data.Curp != null && data.Curp.Length == 18 ? data.Curp.Substring(10, 1) : ND;
            data.ElectorKey = preOcr.ClaveElector;
            data.AnioRegistro = preOcr.AnioRegistro;
            var bornDate = data.Curp != null && data.Curp.Length == 18 ? data.Curp.Substring(4, 6) : ND;

            if (bornDate != ND)
            {
                DateTime.TryParseExact(bornDate, "yyMMdd", null, DateTimeStyles.None, out var bnDate);
                data.BirthDate = bnDate.ToShortDateString();
            }
            else
            {
                data.BirthDate = ND;
            }

            data.Nationality = "MEXICANA";
            data.DocumentType = "IDENTIFICACION OFICIAL";
            data.FrontImage = base64;
            data.FaceImage = Constantes.avatarDefault;
            data.State = preOcr.Estado;
            data.Municipality = preOcr.Municipio;
            data.Locality = preOcr.Localidad;
            data.Section = preOcr.Seccion;
            data.Emission = preOcr.Emision;
            data.ExpiryDate = preOcr.Vigencia;

            var validacionesPorCurp = data.Curp is { Length: 18 };
            var validacionesPorClaveElector = data.ElectorKey is { Length: 18 };
            var validacionesPorFechaNacimiento = data.BirthDate != ND;

            lockWasTaken = true;
            await Task.Factory.StartNew(async () => await _repositoryDocs.AddItemAsyncWithReturn(data))
            .ContinueWith(antecedent =>
            {
                var insertedId = antecedent.Result.Result;
                if (insertedId <= 0) return;

                ((ChatMainViewModel)BindingContext).ReloadData();
                lockWasTaken = false;

                var person = data;
                if (person != null)
                {
                    if (data.EnableIneValidation)
                    {
                        var validaIneRequest = new ValidaIneRequest
                        {
                            IdCiudadano = data.CitizenId,
                            Cic = data.DocumentNumber
                        };

                        //_ = Task.Factory.StartNew(async () => await _repositoryRetrys.UpdateIneAsync(validaIneRequest, person));
                    }
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Ocurrió un error sin data.");
                }
            });

            DependencyService.Get<ILodingPageService>().HideLoadingPage();
            await Task.CompletedTask;
        }

        private async Task<OcrResponse> NewOcr(OcrRequest ocrRequest)
        {
            try
            {
                var result = await _flurlClient
                    .Request(uploadImageUrl)
                    .PostJsonAsync(ocrRequest)
                    .ReceiveJson<OcrResponse>();

                return result;
            }
            catch
            {
            }

            return null;
        }

    }
}
