namespace CHEJ_Shop.UIForms.Helpers
{
    using Common.Resources;
    using Interfaces;
    using Xamarin.Forms;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        #region Labels

        public static string LblAccept => Resource.LblAccept;

        public static string LblBack => Resource.LblBack;

        public static string LblCancel => Resource.LblCancel;

        public static string LblCamera => Resource.LblCamera;

        public static string LblDelete => Resource.LblDelete;

        public static string LblEmail => Resource.LblEmail;

        public static string LblError => Resource.LblError;

        public static string LblForgotYourPassword => Resource.LblForgotYourPassword;

        public static string LblFrom => Resource.LblFrom;

        public static string LblGallery => Resource.LblGallery;

        public static string LblLogin => Resource.LblLogin;

        public static string LblPassword => Resource.LblPassword;

        public static string LblRegisterNewUser => Resource.LblRegisterNewUser;

        public static string LblSave => Resource.LblSave;

        public static string LblUser => Resource.LblUser;

        public static string LblWarning => Resource.LblWarning;

        public static string LblWellcome => Resource.LblWellcome;

        #endregion Labels

        #region Messages

        public static string MsgMustEnterEmail => Resource.MsgMustEnterEmail;

        public static string MsgMustEnterPassword => Resource.MsgMustEnterPassword;

        public static string MsgEmailPasswordIncorrect => Resource.MsgEmailPasswordIncorrect;

        public static string MsgWhereTakePicture => Resource.MsgWhereTakePicture;

        #endregion Messages

        #region Placeholder

        public static string PhEnterYourEmail => Resource.PhEnterYourEmail;

        public static string PhEnterYourPassword => Resource.PhEnterYourPassword;

        public static string PhRemembermeThisDevice => Resource.PhRemembermeThisDevice;

        #endregion Placeholder
    }
}