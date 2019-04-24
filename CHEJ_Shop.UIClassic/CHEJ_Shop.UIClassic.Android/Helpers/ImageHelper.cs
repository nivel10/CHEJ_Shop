namespace CHEJ_Shop.UIClassic.Android.Helpers
{
    using CHEJ_Shop.Common.Helpers;
    using Common.Services;
    using global::Android.Graphics;
    using System;
    using System.Net;

    public class ImageHelper
    {
        public static Bitmap GetImageBitmapFromUrl(
            string _url)
        {
            Bitmap imageBitmap = null;

            try
            {
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(_url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
                return imageBitmap;
            }
            catch (Exception)
            {
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(MethodsHelper.GetUrlNotImage);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
                return imageBitmap;
            }
        }
    }
}