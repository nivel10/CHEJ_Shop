namespace CHEJ_Shop.UIForms.ViewModels
{
    using CHEJ_Shop.Common.Models;
    using CHEJ_Shop.UIForms.Helpers;
    using Common.Helpers;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using MethodsHelper = Helpers.MethodsHelper;

    public class RememberPasswordViewModel : BaseViewModel
    {
        #region Attributes

        private bool isRunning;

        private bool isEnabled;

        private readonly ApiService apiService;

        private readonly DialogService dialogService;

        #endregion Attributes

        #region Properties

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

        public string Email { get; set; }

        public ICommand RecoverCommand => new RelayCommand(this.Recover);

        public ICommand BackCommand => new RelayCommand(this.Back);

        #endregion Properties

        #region Constructor

        public RememberPasswordViewModel()
        {
            //  Get instance of class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();

            //  Set status controls
            this.SetStatusControls(true, false);
        }

        #endregion Constructor

        #region Methods

        private async void Recover()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter an email.",
                    "Accept");
                return;
            }

            var response = RegexHelper.IsValidEmail(this.Email);
            if (!response.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter a valid email.",
                    "Accept");
                return;
            }

            this.SetStatusControls(false, true);

            var request = new RecoverPasswordRequest
            {
                Email = this.Email
            };

            response = await this.apiService.RecoverPasswordAsync(
                MethodsHelper.GetUrlAPI,
                "/api",
                "/Account/RecoverPassword",
                request);

            this.SetStatusControls(true, false);

            if (!response.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            await this.dialogService.ShowMessage(
                "Ok",
                response.Message,
                "Accept");

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void SetStatusControls(
           bool _isEnabled,
           bool _isRunning)
        {
            this.IsEnabled = _isEnabled;
            this.IsRunning = _isRunning;
        }

        #endregion Methods
    }
}