using Xamarin.Forms.Xaml;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace CHEJ_Shop.UIForms
{
    using Common.Helpers;
    using Common.Models;
    using Newtonsoft.Json;
    using System;
    using UIForms.ViewModels;
    using Views;
    using Xamarin.Forms;

    public partial class App : Application
    {
        #region Properties

        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }

        #endregion Properties

        #region Constructor

        public App()
        {
            InitializeComponent();

            #region Old Code

            //  MainPage = new MainPage();
            //  MainPage = new LoginPage();
            //  MainPage = new NavigationPage(new LoginPage());

            //  get Instance LoginViewModel
            //  MainViewModel.GetInstance().Login = new LoginViewModel();
            //  this.MainPage = new NavigationPage(new LoginPage()); 

            #endregion Old Code

            if (Settings.IsRemember)
            {
                var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                var user = JsonConvert.DeserializeObject<User>(Settings.User);

                if (token.Expiration > DateTime.Now)
                {
                    MainViewModel.GetInstance().User = user;
                    MainViewModel.GetInstance().Token = token;
                    MainViewModel.GetInstance().UserEmail = Settings.UserEmail;
                    MainViewModel.GetInstance().UserPassword = Settings.UserPassword;
                    MainViewModel.GetInstance().Products = new ProductsViewModel();
                    this.MainPage = new MasterPage();
                    return;
                }
            }
            //  get Instance LoginViewModel
            MainViewModel.GetInstance().Login = new LoginViewModel();
            this.MainPage = new NavigationPage(new LoginPage());
        }

        #endregion Constructor

        #region Methods

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #endregion Methods
    }
}