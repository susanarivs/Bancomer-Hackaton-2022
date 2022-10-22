using WhiteLabel.Models;

namespace WhiteLabel.Services.Facetec
{
    public class MessagesZoom
    {
        public static string ScanningDoneMessageId = "ScanningDone";
        public static string ScannerSetup = "ScannerStatus";

        public class ScanningDoneMessage
        {
            public bool ScanningCancelled { get; set; }
            public ProfileData ProfileObtained { get; set; }
        }

        public class ScannerStatusMessage
        {
            public bool ScannerReady { get; set; }
        }
    }
}
