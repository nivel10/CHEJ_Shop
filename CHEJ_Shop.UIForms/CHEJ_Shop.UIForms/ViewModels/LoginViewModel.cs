namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Helpers;
    using Common.Models;
    using Common.Resources;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Newtonsoft.Json;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;
    using MethodsHelper = Helpers.MethodsHelper;

    public class LoginViewModel : BaseViewModel
    {
        #region Attributes

        private bool isRunning;
        private bool isEnabled;
        private bool isRemember;
        private readonly ApiService apiService;
        private readonly DialogService dialogService;

        #endregion Attributes

        #region Properties

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public bool IsRemember
        {
            get => this.isRemember;
            set => this.SetValue(ref this.isRemember, value);
        }

        #region Commands

        public ICommand LoginCommand => new RelayCommand(this.Login);

        public ICommand RegisterCommand => new RelayCommand(this.Register);

        public ICommand RememberPasswordCommand => new RelayCommand(this.RememberPassword);

        #endregion Commands

        #endregion Properties

        #region Constructor

        public LoginViewModel()
        {
            //this.Email = "nikole.a.herrera.v@gmail.com";
            //this.Password = "123456";

            //  Instance the services
            this.apiService = new ApiService();
            this.dialogService = new DialogService();

            //  Set status controls
            this.SetStatusControls(false, true, true);
        }

        #endregion Constructor

        #region Methods

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Resource.LblWarning,
                    $"{Resource.MsgMustEnterEmail}...!!!",
                    Resource.LblAccept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Resource.LblWarning,
                    $"{Resource.MsgMustEnterPassword}...!!!",
                    Resource.LblAccept);
                return;
            }

            #region Old Code

            //if (!this.Email.Equals("jzuluaga55@gmail.com") ||
            //    !this.Password.Equals("123456"))
            //{
            //    await Application.Current.MainPage.DisplayAlert(
            //        "Warning",
            //        "User or password wrong...!!!",
            //        "Accept");
            //    return;
            //} 

            #endregion Old Code

            //  Set status controls
            this.SetStatusControls(true, false, this.IsRemember);

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var response = await this.apiService.GetTokenAsync(
                MethodsHelper.GetUrlAPI,
                "/Account",
                "/CreateToken",
                request);

            if (!response.IsSuccess)
            {
                //  Set status controls
                this.SetStatusControls(false, true, this.IsRemember);

                await dialogService.ShowMessage(
                    Resource.LblError,
                    $"{Resource.MsgEmailPasswordIncorrect}...!!!",
                    Resource.LblAccept);
                return;
            }

            var token = (TokenResponse)response.Result;

            #region Old Code

            ////  Get Instance ProductViewModel
            //var products = MainViewModel.GetInstance().Products =
            //    new ProductsViewModel();

            //await Application.Current.MainPage.Navigation.PushAsync
            //    (new ProductsPage()); 

            #endregion Old Code

            #region Old Code

            // var token = (TokenResponse)response.Result;
            //var mainViewModel = MainViewModel.GetInstance();
            //mainViewModel.Token = token;
            //mainViewModel.Products = new ProductsViewModel(); 

            #endregion Old Code

            #region Get User by Email

            var response2 = await this.apiService.GetUserByEmailAsync(
                MethodsHelper.GetUrlAPI,
                "/api",
                "/Account/GetUserByEmail",
                this.Email,
                "bearer",
                token.Token);

            //  Set status controls
            this.SetStatusControls(false, true, this.IsRemember);

            if (!response2.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    Resource.LblError,
                    response2.Message,
                    Resource.LblAccept);
                return;
            }

            var user = (User)response2.Result;

            #endregion Get User by Email

            #region Save Settings User (remember this devices)

            Settings.IsRemember = this.IsRemember;
            Settings.UserEmail = this.Email;
            Settings.UserPassword = this.Password;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.User = JsonConvert.SerializeObject(user);

            #endregion Save Settings User (remember this devices)

            #region Save Data Persistence

            MainViewModel.GetInstance().Token = token;
            MainViewModel.GetInstance().Products = new ProductsViewModel();
            MainViewModel.GetInstance().User = user;
            MainViewModel.GetInstance().UserEmail = this.Email;
            MainViewModel.GetInstance().UserPassword = this.Password;

            #endregion Save Data Persistence

            //  await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());
            Application.Current.MainPage = new MasterPage();
        }

        private void SetStatusControls(
            bool _isRunning,
            bool _isEnabled,
            bool _isRemember)
        {
            this.IsRunning = _isRunning;
            this.IsEnabled = _isEnabled;
            this.IsRemember = _isRemember;
        }

        public async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        public async void RememberPassword()
        {
            MainViewModel.GetInstance().RememberPassword = new RememberPasswordViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(
                new RememberPasswordPage());
        }

        #endregion Methods
    }
}