namespace CHEJ_Shop.UIForms.Helpers
{
    using Xamarin.Forms;

    public class MethodsHelper
    {
        public static string GetUrlAPI => Application.Current.Resources["UrlAPI"].ToString();

        public static string GetUrlNotImage => $"{GetUrlAPI}/images/NoImage.png";
    }
}