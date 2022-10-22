using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Plugin.CurrentActivity;
using WhiteLabel.Core;
using WhiteLabel.Droid.Services.Processors;
using WhiteLabel.Droid.Services;
using Xamarin.Forms.Platform.Android;

namespace WhiteLabel.Droid
{
    [Activity(
        Label = "@string/app_name",
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        MainLauncher = true,
        //LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale | ConfigChanges.LayoutDirection
        )
    ]
    public class MainActivity : FormsAppCompatActivity, IZoomScannerAndroidHostActivity
    {
        public AndroidZoomPlatform CurrentZoomImplementation;

        public Activity HostActivity => this;

        public int ScanZoomActivityRequestCode => 1002;

        public IProcessor latestProcessor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.Window?.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.AppTheme);

            FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar;

            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            ZoomScannerFactoryImplementation.AndroidHostActivity = this;

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            AiForms.Renderers.Droid.SettingsViewInit.Init();
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            UiKit.Init(this);
            LoadApplication(new App());
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            UiKit.NotifyConfigurationChanged(newConfig);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            if (latestProcessor == null)
            {
                return;
            }

            var realizado = latestProcessor.isSuccess();
            var resultado = realizado ? Result.Ok : Result.Canceled;
            if (requestCode == 1002)
            {
                CurrentZoomImplementation.OnActivityResult(requestCode, resultado, data);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void ScanningStarted(AndroidZoomPlatform implementation, string theToken)
        {
            CurrentZoomImplementation = implementation;
            
            latestProcessor = new PhotoIDMatchProcessor(this, theToken);
        }

    }
}

