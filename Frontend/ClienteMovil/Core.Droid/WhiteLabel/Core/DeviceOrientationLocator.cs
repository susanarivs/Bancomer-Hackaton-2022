using Android.App;

namespace WhiteLabel.Core
{
    internal class DeviceOrientationLocator : IDeviceOrientationServiceLocator
    {
        public IDeviceOrientation Service => DeviceOrientationImpl.Instance;

        internal static void Init(Activity activity)
        {
            DeviceOrientationImpl.Instance.Activity = activity;
        }

        public static void NotifyOrientationChanged()
        {
            DeviceOrientationImpl.Instance.ProcessOrientationChange();
        }
    }
}
