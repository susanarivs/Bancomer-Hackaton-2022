using WhiteLabel.Services.Facetec;
using Xamarin.Forms;

namespace WhiteLabel.Droid.Services
{
    internal class FacetecInitializeCallback : Com.Facetec.Sdk.FaceTecSDK.InitializeCallback
    {
        private readonly MainActivity _activity;

        public FacetecInitializeCallback(MainActivity activity)
        {
            _activity = activity;
        }

        public override void OnCompletion(bool p0)
        {
            _activity.RunOnUiThread(delegate
            {
                var statusStr = Com.Facetec.Sdk.FaceTecSDK.GetStatus(_activity).ToString();
                var ready = p0;

                MessagingCenter.Send(new MessagesZoom.ScannerStatusMessage { ScannerReady = ready }, MessagesZoom.ScannerSetup);
            });
        }
    }
}