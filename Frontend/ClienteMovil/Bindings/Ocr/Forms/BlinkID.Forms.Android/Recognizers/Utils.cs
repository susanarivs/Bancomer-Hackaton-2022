using System.IO;
using Android.Graphics;
using Xamarin.Forms;
namespace Microblink.Forms.Droid.Recognizers
{
    public abstract class Utils
    {
        public static ImageSource ConvertAndroidBitmap(Bitmap bitmap) 
        {
            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                bitmapData = stream.ToArray();
            }

            return ImageSource.FromStream(() => new MemoryStream(bitmapData));
        }
    }
}
