namespace CHEJ_Shop.UIForms.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using Plugin.CurrentActivity;
    using Plugin.Permissions;

    #region Old Code

    //[Activity(
    //    Label = "CHEJ_Shop.UIForms",
    //    Icon = "@mipmap/icon",
    //    Theme = "@style/MainTheme",
    //    MainLauncher = true,
    //    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)] 

    #endregion Old Code

    [Activity(
        Label = "CHEJ_Shop - Forms",
        Icon = "@mipmap/ic_chej_shop",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //  CHEJ - Intialize pictute
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        //  CHEJ - Intialize pictute
        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(
                requestCode,
                permissions,
                grantResults);
        }
    }
}