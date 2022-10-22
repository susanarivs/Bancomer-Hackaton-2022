using Android.App;
using Android.Content;
using Com.Facetec.Sdk;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using WhiteLabel.Droid.Services;
using WhiteLabel.Services.Facetec;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidZoomPlatform))]
namespace WhiteLabel.Droid.Services
{
    public sealed class AndroidZoomPlatform : IZoomSdk
    {
        readonly IZoomScannerAndroidHostActivity _androidHostActivity;

        public AndroidZoomPlatform(IZoomScannerAndroidHostActivity androidHostActivity)
        {
            _androidHostActivity = androidHostActivity;

            //personalizacion de ui
            int primaryColor = Android.Graphics.Color.ParseColor("#1c62c5");    //negro 2B2B2B
            int secondaryColor = Android.Graphics.Color.ParseColor("#3BC371");  //verde
            int backgroundColor = Android.Graphics.Color.ParseColor("#EEF6F8"); //blanco
            int buttonBackgroundDisabledColor = Android.Graphics.Color.ParseColor("#adadad");

            var customization = new FaceTecCustomization();
            // Overlay Customization
            customization.OverlayCustomization.BackgroundColor = backgroundColor;
            customization.OverlayCustomization.ShowBrandingImage = false;
            customization.OverlayCustomization.BrandingImage = 0;
            // Guidance Customization
            customization.GuidanceCustomization.BackgroundColors = backgroundColor;
            customization.GuidanceCustomization.ForegroundColor = primaryColor;
            customization.GuidanceCustomization.ButtonTextNormalColor = backgroundColor;
            customization.GuidanceCustomization.ButtonBackgroundNormalColor = primaryColor;
            customization.GuidanceCustomization.ButtonTextHighlightColor = backgroundColor;
            customization.GuidanceCustomization.ButtonBackgroundHighlightColor = Android.Graphics.Color.ParseColor("#565656");
            customization.GuidanceCustomization.ButtonTextDisabledColor = backgroundColor;
            customization.GuidanceCustomization.ButtonBackgroundDisabledColor = buttonBackgroundDisabledColor;
            customization.GuidanceCustomization.ButtonBorderColor = Android.Graphics.Color.Transparent;
            customization.GuidanceCustomization.ButtonBorderWidth = 0;
            customization.GuidanceCustomization.ButtonCornerRadius = 30;
            customization.GuidanceCustomization.ReadyScreenOvalFillColor = Android.Graphics.Color.Transparent;
            customization.GuidanceCustomization.ReadyScreenTextBackgroundColor = backgroundColor;
            customization.GuidanceCustomization.ReadyScreenTextBackgroundCornerRadius = 5;
            customization.GuidanceCustomization.RetryScreenImageBorderColor = primaryColor;
            customization.GuidanceCustomization.RetryScreenImageBorderWidth = 2;
            customization.GuidanceCustomization.RetryScreenImageCornerRadius = 10;
            customization.GuidanceCustomization.RetryScreenOvalStrokeColor = backgroundColor;
            customization.GuidanceCustomization.RetryScreenSlideshowInterval = 2000;
            customization.GuidanceCustomization.EnableRetryScreenSlideshowShuffle = true;
            // ID Scan Customization
            customization.IdScanCustomization.ShowSelectionScreenDocumentImage = true;
            customization.IdScanCustomization.ShowSelectionScreenBrandingImage = false;
            customization.IdScanCustomization.SelectionScreenBrandingImage = 0;
            customization.IdScanCustomization.SelectionScreenBackgroundColors = backgroundColor;
            customization.IdScanCustomization.ReviewScreenBackgroundColors = backgroundColor;
            customization.IdScanCustomization.CaptureScreenForegroundColor = primaryColor;
            customization.IdScanCustomization.ReviewScreenForegroundColor = primaryColor;
            customization.IdScanCustomization.SelectionScreenForegroundColor = primaryColor;
            customization.IdScanCustomization.CaptureScreenFocusMessageTextColor = Android.Graphics.Color.ParseColor("#565656");
            customization.IdScanCustomization.ButtonTextNormalColor = backgroundColor;
            customization.IdScanCustomization.ButtonBackgroundNormalColor = primaryColor;
            customization.IdScanCustomization.ButtonTextHighlightColor = backgroundColor;
            customization.IdScanCustomization.ButtonBackgroundHighlightColor = Android.Graphics.Color.ParseColor("#565656");
            customization.IdScanCustomization.ButtonTextDisabledColor = backgroundColor;
            customization.IdScanCustomization.ButtonBackgroundDisabledColor = buttonBackgroundDisabledColor;
            customization.IdScanCustomization.ButtonBorderColor = Android.Graphics.Color.Transparent;
            customization.IdScanCustomization.ButtonBorderWidth = 0;
            customization.IdScanCustomization.ButtonCornerRadius = 30;
            customization.IdScanCustomization.CaptureScreenTextBackgroundColor = backgroundColor;
            customization.IdScanCustomization.CaptureScreenTextBackgroundBorderColor = primaryColor;
            customization.IdScanCustomization.CaptureScreenTextBackgroundBorderWidth = 2;
            customization.IdScanCustomization.CaptureScreenTextBackgroundCornerRadius = 5;
            customization.IdScanCustomization.ReviewScreenTextBackgroundColor = backgroundColor;
            customization.IdScanCustomization.ReviewScreenTextBackgroundBorderColor = primaryColor;
            customization.IdScanCustomization.ReviewScreenTextBackgroundBorderWidth = 2;
            customization.IdScanCustomization.ReviewScreenTextBackgroundCornerRadius = 5;
            customization.IdScanCustomization.CaptureScreenBackgroundColor = backgroundColor;
            customization.IdScanCustomization.CaptureFrameStrokeColor = primaryColor;
            customization.IdScanCustomization.CaptureFrameStrokeWidth = 2;
            customization.IdScanCustomization.CaptureFrameCornerRadius = 12;
            // OCR Confirmation Screen Customization
            customization.OcrConfirmationCustomization.BackgroundColors = backgroundColor;
            customization.OcrConfirmationCustomization.MainHeaderDividerLineColor = secondaryColor;
            customization.OcrConfirmationCustomization.MainHeaderDividerLineWidth = 2;
            customization.OcrConfirmationCustomization.MainHeaderTextColor = secondaryColor;
            customization.OcrConfirmationCustomization.SectionHeaderTextColor = primaryColor;
            customization.OcrConfirmationCustomization.FieldLabelTextColor = primaryColor;
            customization.OcrConfirmationCustomization.FieldValueTextColor = primaryColor;
            customization.OcrConfirmationCustomization.InputFieldTextColor = primaryColor;
            customization.OcrConfirmationCustomization.InputFieldPlaceholderTextColor = Android.Graphics.Color.ParseColor("#663BC371");
            customization.OcrConfirmationCustomization.InputFieldBackgroundColor = Android.Graphics.Color.Transparent;
            customization.OcrConfirmationCustomization.InputFieldBorderColor = secondaryColor;
            customization.OcrConfirmationCustomization.InputFieldBorderWidth = 2;
            customization.OcrConfirmationCustomization.InputFieldCornerRadius = 0;
            customization.OcrConfirmationCustomization.ShowInputFieldBottomBorderOnly = true;
            customization.OcrConfirmationCustomization.ButtonTextNormalColor = backgroundColor;
            customization.OcrConfirmationCustomization.ButtonBackgroundNormalColor = primaryColor;
            customization.OcrConfirmationCustomization.ButtonTextHighlightColor = backgroundColor;
            customization.OcrConfirmationCustomization.ButtonBackgroundHighlightColor = Android.Graphics.Color.ParseColor("#565656");
            customization.OcrConfirmationCustomization.ButtonTextDisabledColor = backgroundColor;
            customization.OcrConfirmationCustomization.ButtonBackgroundDisabledColor = buttonBackgroundDisabledColor;
            customization.OcrConfirmationCustomization.ButtonBorderColor = Android.Graphics.Color.Transparent;
            customization.OcrConfirmationCustomization.ButtonBorderWidth = 0;
            customization.OcrConfirmationCustomization.ButtonCornerRadius = 30;
            // Result Screen Customization
            customization.ResultScreenCustomization.BackgroundColors = backgroundColor;
            customization.ResultScreenCustomization.ForegroundColor = primaryColor;
            customization.ResultScreenCustomization.ActivityIndicatorColor = primaryColor;
            customization.ResultScreenCustomization.CustomActivityIndicatorImage = 0;
            customization.ResultScreenCustomization.CustomActivityIndicatorRotationInterval = 800;
            customization.ResultScreenCustomization.ResultAnimationBackgroundColor = secondaryColor;
            customization.ResultScreenCustomization.ResultAnimationForegroundColor = backgroundColor;
            customization.ResultScreenCustomization.ResultAnimationSuccessBackgroundImage = 0;
            customization.ResultScreenCustomization.ResultAnimationUnsuccessBackgroundImage = 0;
            customization.ResultScreenCustomization.ShowUploadProgressBar = true;
            customization.ResultScreenCustomization.UploadProgressTrackColor = Android.Graphics.Color.ParseColor("#332B2B2B");
            customization.ResultScreenCustomization.UploadProgressFillColor = secondaryColor;
            customization.ResultScreenCustomization.AnimationRelativeScale = 1.0f;
            // Feedback Customization
            customization.FeedbackCustomization.BackgroundColors = secondaryColor;
            customization.FeedbackCustomization.TextColor = backgroundColor;
            customization.FeedbackCustomization.CornerRadius = 5;
            customization.FeedbackCustomization.Elevation = 10;
            // Frame Customization
            customization.FrameCustomization.BackgroundColor = backgroundColor;
            customization.FrameCustomization.BorderColor = primaryColor;
            customization.FrameCustomization.BorderWidth = 0;
            customization.FrameCustomization.CornerRadius = 0;
            customization.FrameCustomization.Elevation = 0;
            // Oval Customization
            customization.OvalCustomization.StrokeColor = primaryColor;
            customization.OvalCustomization.ProgressColor1 = Android.Graphics.Color.ParseColor("#BF3BC371");
            customization.OvalCustomization.ProgressColor2 = Android.Graphics.Color.ParseColor("#BF3BC371");

            // Guidance Customization -- Text Style Overrides
            // Ready Screen Header
            customization.GuidanceCustomization.ReadyScreenHeaderTextColor = primaryColor;
            // Ready Screen Subtext
            customization.GuidanceCustomization.ReadyScreenSubtextTextColor = Android.Graphics.Color.ParseColor("#565656");
            // Ready Screen Header
            customization.GuidanceCustomization.RetryScreenHeaderTextColor = primaryColor;
            // Retry Screen Subtext
            customization.GuidanceCustomization.RetryScreenSubtextTextColor = Android.Graphics.Color.ParseColor("#565656");
            FaceTecSDK.SetCustomization(customization);
            
            var context = Android.App.Application.Context;
            FaceTecSDK.InitializeInDevelopmentMode(context, FacetecConsts.DeviceKeyIdentifier, FacetecConsts.PublicFaceScanEncryptionKey,
                new FacetecInitializeCallback((MainActivity)_androidHostActivity.HostActivity));
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == _androidHostActivity.ScanZoomActivityRequestCode)
            {
                if (resultCode == Result.Ok)
                {
                    MessagingCenter.Send(new MessagesZoom.ScanningDoneMessage { ScanningCancelled = false }, MessagesZoom.ScanningDoneMessageId);
                }
                else
                {
                    MessagingCenter.Send(new MessagesZoom.ScanningDoneMessage { ScanningCancelled = true }, MessagesZoom.ScanningDoneMessageId);
                }
            }
        }

        public void Scan()
        {
            //Ir por el token
            var theToken = ObtenerToken();
            _androidHostActivity.ScanningStarted(this, theToken);
        }

        private string ObtenerToken()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Device-Key", FacetecConsts.DeviceKeyIdentifier);
            client.DefaultRequestHeaders.Add("X-User-Agent", FaceTecSDK.CreateFaceTecAPIUserAgentString(""));

            var response = client.GetAsync(FacetecConsts.BaseURL + "/session-token").Result;
            var result = response.Content.ReadAsStringAsync().Result;
            dynamic parsedResponse = JObject.Parse(result);

            return parsedResponse.sessionToken;
        }

    }
}