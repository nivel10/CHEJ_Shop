namespace CHEJ_Shop.Common.ViewModels
{
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.ViewModels;
    using System.Windows.Input;

    public class LoginViewModel : MvxViewModel
    {
        private string email;
        private string password;
        private MvxCommand loginCommand;
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private bool isLoading;

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public string Email
        {
            get => this.email;
            set => this.SetProperty(ref this.email, value);
        }

        public string Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        public ICommand LoginCommand
        {
            get
            {
                this.loginCommand = this.loginCommand ?? new MvxCommand(this.DoLoginCommand);
                return this.loginCommand;
            }
        }

        public LoginViewModel(
            IApiService _apiService,
            IDialogService _dialogService)
        {
            this.apiService = _apiService;
            this.dialogService = _dialogService;

            this.Email = "chejconsultor@gmail.com";
            this.Password = "Chej5654.";
            this.IsLoading = false;
        }

        private async void DoLoginCommand()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                this.dialogService.Alert("Error", "You must enter an email.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                this.dialogService.Alert("Error", "You must enter a password.", "Accept");
                return;
            }

            this.IsLoading = true;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var response = await this.apiService.GetTokenAsync(
                "http://chejconsultor.ddns.net:9002",
                "/Account",
                "/CreateToken",
                request);

            if (!response.IsSuccess)
            {
                this.IsLoading = false;
                this.dialogService.Alert(
                    "Error",
                    "User or password incorrect.",
                    "Accept");
                return;
            }

            this.IsLoading = false;
            this.dialogService.Alert(
                "Ok",
                "Fuck yeah!",
                "Accept");
        }
    }
}