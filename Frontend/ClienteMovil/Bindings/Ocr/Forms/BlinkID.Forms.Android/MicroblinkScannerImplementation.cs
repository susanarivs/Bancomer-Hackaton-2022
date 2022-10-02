﻿using Android.App;
using Android.Content;
using Com.Microblink;
using Com.Microblink.Entities.Recognizers;
using Com.Microblink.Intent;
using Com.Microblink.Uisettings;
using Microblink.Forms.Core;
using Microblink.Forms.Core.Overlays;
using Microblink.Forms.Droid;
using Microblink.Forms.Droid.Overlays;
using Microblink.Forms.Droid.Recognizers;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(MicroblinkScannerFactoryImplementation))]
namespace Microblink.Forms.Droid
{
    public interface IMicroblinkScannerAndroidHostActivity
    {
        /// <summary>
        /// Returns the host activity that is currently in use.
        /// </summary>
        /// <value>The host activity.</value>
        Activity HostActivity { get; }

        /// <summary>
        /// Gets the scan activity request code. You can define your custom request code
        /// so that it will not interfere with request codes your app uses with other
        /// activities.
        /// </summary>
        /// <value>The scan activity request code.</value>
        int ScanBlinkActivityRequestCode { get; }

        /// <summary>
        /// 
        /// </summary>
        int ScanBlinkActivityBackRequestCode { get; }

        /// <summary>
        /// This method is called from Android's version of MicroblinkScannerImplementation at
        /// the time when scanning will be started. You should save the implementation's object
        /// reference here and use it in OnActivityResult method to forward that event to it.
        /// </summary>
        /// <param name="implementation">Implementation.</param>
        void ScanningStarted(MicroblinkScannerImplementation implementation);
    }

    public sealed class MicroblinkScannerImplementation : IMicroblinkScanner
    {
        readonly IMicroblinkScannerAndroidHostActivity androidHostActivity;

        RecognizerBundle recognizerBundle;

        public MicroblinkScannerImplementation(IMicroblinkScannerAndroidHostActivity androidHostActivity)
        {
            byte[] officialBuff = new byte[] { (byte)0xb1, (byte)0x1c, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x19,
                (byte)0x63, (byte)0x6f, (byte)0x6d, (byte)0x2e, (byte)0x6d, (byte)0x69, (byte)0x63, (byte)0x72, (byte)0x6f, (byte)0x62, (byte)0x6c, (byte)0x69, (byte)0x6e, (byte)0x6b, (byte)0x2e, (byte)0x62, (byte)0x6c, (byte)0x69, (byte)0x6e, (byte)0x6b, (byte)0x69, (byte)0x64, (byte)0x61, (byte)0x70, (byte)0x70,
                (byte)0x89, (byte)0x5b, (byte)0xdb, (byte)0x9d, (byte)0x65, (byte)0x12, (byte)0xfb, (byte)0x9c, (byte)0xce, (byte)0x71, (byte)0x0c, (byte)0x41, (byte)0x1d, (byte)0xef, (byte)0x5d, (byte)0xce,
                (byte)0xeb, (byte)0xbe, (byte)0xca, (byte)0x0c, (byte)0xf4, (byte)0xec, (byte)0xf7, (byte)0x34, (byte)0x83, (byte)0x88, (byte)0x87, (byte)0x6c, (byte)0x6d, (byte)0x78, (byte)0x0f, (byte)0x3d,
                (byte)0x63, (byte)0x1e, (byte)0xe5, (byte)0xc4, (byte)0xa3, (byte)0x67, (byte)0x03, (byte)0x64, (byte)0x80, (byte)0x24, (byte)0x62, (byte)0x2f, (byte)0x91, (byte)0xab, (byte)0xe5, (byte)0xd0,
                (byte)0x34, (byte)0x26, (byte)0x0a, (byte)0x5d, (byte)0x63, (byte)0x98, (byte)0xf3, (byte)0x50, (byte)0xb5, (byte)0xcb, (byte)0xe7, (byte)0x15, (byte)0xc7, (byte)0x58, (byte)0xa5, (byte)0xac,
                (byte)0x1b, (byte)0xc3, (byte)0xfc, (byte)0xd8, (byte)0x93, (byte)0xd3, (byte)0x8a, (byte)0x7a, (byte)0xb9, (byte)0x2f, (byte)0x7e, (byte)0xae, (byte)0x7d, (byte)0x55, (byte)0x66, (byte)0xad,
                (byte)0x7d, (byte)0x7f, (byte)0x57, (byte)0xce, (byte)0x9d, (byte)0x3e, (byte)0xe7, (byte)0xf1, (byte)0x36, (byte)0x50, (byte)0x04, (byte)0xdc, (byte)0x19, (byte)0xeb, (byte)0x40, (byte)0xd5,
                (byte)0xeb, (byte)0x37, (byte)0x3e, (byte)0x47, (byte)0x32, (byte)0x3a, (byte)0x5e, (byte)0x03, (byte)0x92, (byte)0xa7, (byte)0x01, (byte)0x0f, (byte)0x06, (byte)0xc3, (byte)0xc8, (byte)0xe9,
                (byte)0x31, (byte)0x9f, (byte)0x5e, (byte)0xd8, (byte)0xf6, (byte)0x32, (byte)0x9a, (byte)0x38, (byte)0x25, (byte)0x3f, (byte)0xb5, (byte)0x8e, (byte)0x54, (byte)0xd2, (byte)0x65, (byte)0x76,
                (byte)0x90, (byte)0x6c, (byte)0x48, (byte)0xca, (byte)0xcc, (byte)0x60, (byte)0x0f, (byte)0xd5, (byte)0xb6, (byte)0x06, (byte)0xdf, (byte)0x9d, (byte)0x0f, (byte)0x74, (byte)0xfc, (byte)0x49,
                (byte)0x88, (byte)0xb2, (byte)0x41, (byte)0x39, (byte)0x64, (byte)0xc6, (byte)0x57, (byte)0xab, (byte)0x91, (byte)0x95, (byte)0x18, (byte)0xd3, (byte)0xe0, (byte)0xcb, (byte)0x2f, (byte)0x9f,
                (byte)0x88, (byte)0x60, (byte)0x44, (byte)0x61, (byte)0x7e, (byte)0x38, (byte)0x33, (byte)0xb5, (byte)0xc8, (byte)0x7f, (byte)0x73, (byte)0xc2, (byte)0x21, (byte)0xc9, (byte)0x99, (byte)0x3e,
                (byte)0x70, (byte)0xe3, (byte)0x15, (byte)0xe9, (byte)0x32, (byte)0x79, (byte)0x81, (byte)0x25, (byte)0x29, (byte)0xe8, (byte)0x98, (byte)0x15, (byte)0xdb, (byte)0x12, (byte)0xa5, (byte)0x8e,
                (byte)0x45, (byte)0xa6, (byte)0x5a, (byte)0x5d, (byte)0x2c, (byte)0xf7, (byte)0x74, (byte)0xf1, (byte)0x00, (byte)0x79, (byte)0x31, (byte)0x30, (byte)0x48, (byte)0xba, (byte)0xc3, (byte)0xda,
                (byte)0x0c, (byte)0x60, (byte)0x3c, (byte)0xad, (byte)0xc3, (byte)0x90, (byte)0x1d, (byte)0x99, (byte)0x16, (byte)0x98, (byte)0x6f, (byte)0xb9, (byte)0xc0, (byte)0x87, (byte)0x28, (byte)0x13,
                (byte)0xf3, (byte)0x42, (byte)0x2e, (byte)0xf9, (byte)0x0a, (byte)0xb0, (byte)0xb0, (byte)0x23, (byte)0x4a, (byte)0x01, (byte)0x96, (byte)0xe9, (byte)0x36, (byte)0x3d, (byte)0xbb, (byte)0x45,
                (byte)0x60, (byte)0x6f, (byte)0xbf, (byte)0x8d, (byte)0xd5, (byte)0x07, (byte)0x00, (byte)0x81, (byte)0x15, (byte)0x6d, (byte)0x73, (byte)0x5c, (byte)0x58, (byte)0x9b, (byte)0x12, (byte)0x90,
                (byte)0x77, (byte)0x34, (byte)0x68, (byte)0x67, (byte)0x14, (byte)0x10, (byte)0x8c, (byte)0x10, (byte)0x81, (byte)0x44, (byte)0x87, (byte)0x72, (byte)0x67, (byte)0x63, (byte)0xf1, (byte)0x55,
                (byte)0x78, (byte)0x52, (byte)0x4f, (byte)0xa2, (byte)0xa4, (byte)0xc5, (byte)0xc8, (byte)0xca, (byte)0x05, (byte)0x0a, (byte)0x57, (byte)0x07, (byte)0x39, (byte)0xe3, (byte)0xdf, (byte)0x54,
                (byte)0xb5, (byte)0xed, (byte)0x83, (byte)0xb4, (byte)0x07, (byte)0x16, (byte)0xf1, (byte)0x55, (byte)0x66, (byte)0x56, (byte)0x52, (byte)0x82, (byte)0xc0, (byte)0x7d, (byte)0x3a, (byte)0x28,
                (byte)0x75, (byte)0x93, (byte)0x29, (byte)0x1c, (byte)0xa7, (byte)0x2f, (byte)0xaf, (byte)0x11, (byte)0x62, (byte)0x3b, (byte)0x70, (byte)0xb3, (byte)0x8a, (byte)0x3a, (byte)0xda, (byte)0x80,
                (byte)0x97, (byte)0x00, (byte)0x34, (byte)0x3d, (byte)0x94, (byte)0x4d, (byte)0xc1, (byte)0x98, (byte)0x6b, (byte)0x15, (byte)0xa3, (byte)0xc3, (byte)0x5c, (byte)0x31, (byte)0xc3, (byte)0x9b,
                (byte)0xe7
        };

            this.androidHostActivity = androidHostActivity;
            MicroblinkSDK.SetShowTimeLimitedLicenseWarning(false);
            MicroblinkSDK.SetLicenseBuffer(officialBuff, androidHostActivity.HostActivity);
            _ = MicroblinkSDK.NativeLibraryVersionString;   //version

            MicroblinkSDK.IntentDataTransferMode = IntentDataTransferMode.PersistedOptimised;
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == androidHostActivity.ScanBlinkActivityRequestCode)
            {
                if (resultCode == Result.Ok)
                {
                    recognizerBundle.LoadFromIntent(data);
                    MessagingCenter.Send(new Messages.ScanningDoneMessage { ScanningCancelled = false }, Messages.ScanningDoneMessageId);
                }
                else
                {
                    MessagingCenter.Send(new Messages.ScanningDoneMessage { ScanningCancelled = true }, Messages.ScanningDoneMessageId);
                }
            }

            if (requestCode == androidHostActivity.ScanBlinkActivityBackRequestCode)
            {
                if (resultCode == Result.Ok)
                {
                    recognizerBundle.LoadFromIntent(data);
                    MessagingCenter.Send(new Messages.ScanningBackDoneMessage { ScanningCancelled = false }, Messages.ScanningDoneMessageId);
                }
                else
                {
                    MessagingCenter.Send(new Messages.ScanningBackDoneMessage { ScanningCancelled = true }, Messages.ScanningDoneMessageId);
                }
            }
        }

        public void ScanFront(IOverlaySettings overlaySettings)
        {
            androidHostActivity.ScanningStarted(this);
            var aOverlaySettings = (OverlaySettings)overlaySettings;
            recognizerBundle = ((RecognizerCollection)aOverlaySettings.RecognizerCollection).NativeRecognizerBundle;
            ActivityRunner.StartActivityForResult(androidHostActivity.HostActivity, androidHostActivity.ScanBlinkActivityRequestCode, ((OverlaySettings)overlaySettings).NativeUISettings);
        }

        public void ScanBack(IOverlaySettings overlaySettings)
        {
            androidHostActivity.ScanningStarted(this);
            var aOverlaySettings = (OverlaySettings)overlaySettings;
            recognizerBundle = ((RecognizerCollection)aOverlaySettings.RecognizerCollection).NativeRecognizerBundle;
            ActivityRunner.StartActivityForResult(androidHostActivity.HostActivity, androidHostActivity.ScanBlinkActivityBackRequestCode, ((OverlaySettings)overlaySettings).NativeUISettings);
        }
    }

    public sealed class MicroblinkScannerFactoryImplementation : IMicroblinkScannerFactory
    {
        /// <summary>
        /// Set this to your implementation of IMicroblinkScannerAndroidHostActivity interface before
        /// calling CreateMicroblinkScanner method.
        /// </summary>
        /// <value>The android host activity.</value>
        public static IMicroblinkScannerAndroidHostActivity AndroidHostActivity { get; set; }

        public IMicroblinkScanner CreateMicroblinkScanner()
        {
            if (AndroidHostActivity == null)
            {
                throw new NullReferenceException("Please set AndroidHostActivity implementation in your Droid project.");
            }

            return new MicroblinkScannerImplementation(AndroidHostActivity);
        }
    }
}