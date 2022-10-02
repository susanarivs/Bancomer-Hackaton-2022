using Microblink.Forms.Core.Overlays;

namespace Microblink.Forms.Core
{
    /// <summary>
    /// A main scanner object that will be used for performing the scan.
    /// </summary>
    public interface IMicroblinkScanner
    {
        /// <summary>
        /// Perform the scanning using overlay specified by given IOverlaySettings.
        /// </summary>
        /// <param name="overlaySettings">Overlay settings that specify the UI overlay that will be used for scanning.</param>
        void ScanFront(IOverlaySettings overlaySettings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="overlaySettings"></param>
        void ScanBack(IOverlaySettings overlaySettings);
    }

    /// <summary>
    /// Microblink scanner factory is needed for creating an instance of IMicroblinkScanner.
    /// </summary>
    public interface IMicroblinkScannerFactory
    {
        /// <summary>
        /// Creates the microblink scanner using provided licenseKey. The license key used must be bound to
        /// specific iOS bundleID or Android application ID.
        /// </summary>
        /// <returns>The microblink scanner.</returns>
        IMicroblinkScanner CreateMicroblinkScanner();
    }

}

