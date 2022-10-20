using Android.App;

namespace WhiteLabel.Droid.Services
{
    public interface IZoomScannerAndroidHostActivity
    {
        Activity HostActivity { get; }

        int ScanZoomActivityRequestCode { get; }

        void ScanningStarted(AndroidZoomPlatform implementation, string theToken);
    }
}