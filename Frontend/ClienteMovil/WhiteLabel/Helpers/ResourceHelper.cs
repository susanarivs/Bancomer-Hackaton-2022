using Xamarin.Forms;

namespace WhiteLabel
{
    public static class ResourceHelper
    {
        public static T FindResource<T>(string resourceKey)
        {
            if (Application.Current?.Resources != null &&
                Application.Current.Resources.TryGetValue(resourceKey, out var result))
            {
                if (result is T t)
                {
                    return t;
                }
                else if (result is OnPlatform<T> platform)
                {
                    return platform;
                }
            }

            return default;
        }
    }
}