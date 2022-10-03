using Android.App;
using Android.Widget;
using WhiteLabel.Droid.Services;
using WhiteLabel.Services.Messages;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace WhiteLabel.Droid.Services
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long)?.Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short)?.Show();
        }
    }
}