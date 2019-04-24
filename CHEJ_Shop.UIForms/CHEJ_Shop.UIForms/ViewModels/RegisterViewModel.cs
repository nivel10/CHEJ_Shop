namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    using MethodsHelper = Helpers.MethodsHelper;

    public class RegisterViewModel : BaseViewModel
    {
        #region Attributes

        private readonly ApiService apiService;

        private readonly DialogService dialogService;

        private bool isRunning;

        private bool isEnabled;

        private ObservableCollection<Country> countries;

        private Country country;

        private ObservableCollection<City> cities;

        private City city;

        #endregion Attributes

        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string Confirm { get; set; }

        public Country Country
        {
            get => this.country;
            set
            {
                this.SetValue(ref this.country, value);
                this.Cities = new ObservableCollection<City>(
                    this.Country.Cities.OrderBy(country => country.Name));
            }
        }

        public City City
        {
            get => this.city;
            set => this.SetValue(ref this.city, value);
        }

        public ObservableCollection<Country> Countries
        {
            get => this.countries;
            set => this.SetValue(ref this.countries, value);
        }

        public ObservableCollection<City> Cities
        {
            get => this.cities;
            set => this.SetValue(ref this.cities, value);
        }

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

        public ICommand RegisterCommand => new RelayCommand(this.Register);

        public ICommand BackCommand => new RelayCommand(this.Back);

        #endregion Commands

        #endregion Properties

        #region Constructor

        public RegisterViewModel()
        {
            //  Get instances of class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();

            //  Load countries
            this.LoadCountries();
        }

        private async void LoadCountries()
        {
            this.SetStatusControls(false, true);

            var response = await apiService.GetListAsync<Country>(
                MethodsHelper.GetUrlAPI,
                "/api",
                "/Countries");

            this.SetStatusControls(true, false);

            if (!response.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    Common.Resources.Resource.LblError,
                    response.Message,
                    Common.Resources.Resource.LblAccept);
                return;
            }

            var countriesList = (List<Country>)response.Result;
            this.Countries = new ObservableCollection<Country>(countriesList);
        }

        #endregion Constructor

        #region Methods

        private async void Register()
        {
            try
            {
                if (string.IsNullOrEmpty(this.FirstName))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter the first name.",
                        "Accept");
                    return;
                }

                if (string.IsNullOrEmpty(this.LastName))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter the last name.",
                        "Accept");
                    return;
                }

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
                        Common.Resources.Resource.LblError,
                        response.Message,
                        Common.Resources.Resource.LblAccept);
                    return;
                }

                if (this.Country == null)
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must select a country.",
                        "Accept");
                    return;
                }

                if (this.City == null)
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must select a city.",
                        "Accept");
                    return;
                }

                if (string.IsNullOrEmpty(this.Address))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter an address.",
                        "Accept");
                    return;
                }

                if (string.IsNullOrEmpty(this.Phone))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter a phone number.",
                        "Accept");
                    return;
                }

                if (string.IsNullOrEmpty(this.Password))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter a password.",
                        "Accept");
                    return;
                }

                if (this.Password.Length < 6)
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You password must be at mimimun 6 characters.",
                        "Accept");
                    return;
                }

                if (string.IsNullOrEmpty(this.Confirm))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter a password confirm.",
                        "Accept");
                    return;
                }

                if (!this.Password.Equals(this.Confirm))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "The password and the confirm do not match.",
                        "Accept");
                    return;
                }

                //  Set status controls
                this.SetStatusControls(false, true);

                var request = new NewUserRequest
                {
                    Address = this.Address,
                    CityId = this.City.Id,
                    Email = this.Email,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Password = this.Password,
                    Phone = this.Phone
                };

                response = await this.apiService.RegisterUserAsync(
                    MethodsHelper.GetUrlAPI,
                    "/api",
                    "/Account",
                    request);

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

                await this.dialogService.ShowMessage(
                    "Ok",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();

            }
            catch (Exception ex)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    ex.Message,
                    "Accept");
                return;
            }
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