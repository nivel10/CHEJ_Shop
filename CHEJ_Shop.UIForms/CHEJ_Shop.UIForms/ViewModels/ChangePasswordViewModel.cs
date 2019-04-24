namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using MethodsHelper = Helpers.MethodsHelper;

    public class ChangePasswordViewModel : BaseViewModel
    {
        #region Attributes

        private readonly ApiService apiService;

        private readonly DialogService dialogService;

        private bool isRunning;

        private bool isEnabled;

        #endregion Attributes

        #region Properties

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string PasswordConfirm { get; set; }

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

        #region Commands

        public ICommand ChangePasswordCommand => new RelayCommand(this.ChangePassword);

        public ICommand BackCommand => new RelayCommand(this.Back);

        #endregion Commands

        #endregion Properties

        #region Constructor

        public ChangePasswordViewModel()
        {
            //  Get new instance of class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();

            //  Set status controls
            this.SetStatusControls(true, false);
        }

        #endregion Constructor

        #region Methods

        private async void ChangePassword()
        {
            if (string.IsNullOrEmpty(this.CurrentPassword))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter the current password.",
                    "Accept");
                return;
            }

            if (!MainViewModel.GetInstance().UserPassword.Equals(this.CurrentPassword))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "The current password is incorrect.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.NewPassword))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter the new password.",
                    "Accept");
                return;
            }

            if (this.NewPassword.Length < 6)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "The password must have at least 6 characters length.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.PasswordConfirm))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter the password confirm.",
                    "Accept");
                return;
            }

            if (!this.NewPassword.Equals(this.PasswordConfirm))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "The password and confirm does not match.",
                    "Accept");
                return;
            }

            //  Set status controls
            this.SetStatusControls(false, true);

            var request = new ChangePasswordRequest
            {
                Email = MainViewModel.GetInstance().UserEmail,
                NewPassword = this.NewPassword,
                OldPassword = this.CurrentPassword
            };

            var response = await this.apiService.ChangePasswordAsync(
                MethodsHelper.GetUrlAPI,
                "/api",
                "/Account/ChangePassword",
                request,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            //  Set status controls
            this.SetStatusControls(true, false);

            if (!response.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            MainViewModel.GetInstance().UserPassword = this.NewPassword;
            Settings.UserPassword = this.NewPassword;

            await this.dialogService.ShowMessage(
                "Ok",
                response.Message,
                "Accept");

            await App.Navigator.PopAsync();
        }

        private async void Back()
        {
            await App.Navigator.PopAsync();
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