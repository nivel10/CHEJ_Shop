namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using UIForms.Helpers;
    using Views;
    using MethodsHelper = Helpers.MethodsHelper;

    public class ProfileViewModel : BaseViewModel
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

        private User user;

        private List<Country> myCountries;

        #endregion Attributes

        #region Properties

        public Country Country
        {
            get => this.country;
            set
            {
                this.SetValue(ref this.country, value);
                this.Cities = new ObservableCollection<City>(this.Country.Cities.OrderBy(c => c.Name));
            }
        }

        public City City
        {
            get => this.city;
            set => this.SetValue(ref this.city, value);
        }

        public User User
        {
            get => this.user;
            set => this.SetValue(ref this.user, value);
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

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand ModifyPasswordCommand => new RelayCommand(this.ModifyPassword);

        public ICommand BackCommand => new RelayCommand(this.Back);

        #endregion Properties

        #region Constructor

        public ProfileViewModel()
        {
            //  Get a instance of class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();

            this.User = MainViewModel.GetInstance().User;

            //  Set Status Controls
            this.SetStatusControls(true, false);

            //  Load Countries
            this.LoadCountries();
        }

        #endregion Constructor

        #region Methods

        private async void LoadCountries()
        {
            this.SetStatusControls(false, true);

            var response = await this.apiService.GetListAsync<Country>(
                MethodsHelper.GetUrlAPI,
                "/api",
                "/Countries");

            this.SetStatusControls(true, false);

            if (!response.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.myCountries = (List<Country>)response.Result;
            this.Countries = new ObservableCollection<Country>(myCountries);
            this.SetCountryAndCity();
        }

        private void SetCountryAndCity()
        {
            foreach (var country in this.myCountries)
            {
                var city = country.Cities
                    .Where(c => c.Id == this.User.CityId)
                    .FirstOrDefault();
                if (city != null)
                {
                    this.Country = country;
                    this.City = city;
                    return;
                }
            }
        }

        private void SetStatusControls(
           bool _isEnabled,
           bool _isRunning)
        {
            this.IsEnabled = _isEnabled;
            this.IsRunning = _isRunning;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.User.FirstName))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter the first name.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.User.LastName))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter the last name.",
                    "Accept");
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

            if (string.IsNullOrEmpty(this.User.Address))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter an address.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.User.PhoneNumber))
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter a phone number.",
                    "Accept");
                return;
            }

            this.SetStatusControls(false, true);

            //  Set CytiId
            this.User.CityId = this.City.Id;

            var response = await this.apiService.PutAsync(
                MethodsHelper.GetUrlAPI,
                "/api",
                "/Account",
                this.User,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.SetStatusControls(true, false);

            if (!response.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            MainViewModel.GetInstance().User = this.User;
            Settings.User = JsonConvert.SerializeObject(this.User);

            await this.dialogService.ShowMessage(
                "Ok",
                "User updated...!!!!",
                "Accept");
            await App.Navigator.PopAsync();
        }

        private async void ModifyPassword()
        {
            MainViewModel.GetInstance().ChangePassword = new ChangePasswordViewModel();
            await App.Navigator.PushAsync(new ChangePasswordPage());
        }

        private async void Back()
        {
            await App.Navigator.PopAsync();
        }

        #endregion Methods
    }
}