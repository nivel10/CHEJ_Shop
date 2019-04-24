namespace CHEJ_Shop.UIForms.ViewModels
{
    using CHEJ_Shop.Common.Helpers;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class MenuItemViewModel : Common.Models.Menu
    {
        #region Properties

        public ICommand SelectMenuCommand => new RelayCommand(this.SelectMenu);

        #endregion Properties

        #region Methods

        private async void SelectMenu()
        {
            App.Master.IsPresented = false;

            switch (this.PageName)
            {
                case "AboutPage":
                    await App.Navigator.PushAsync(new AboutPage());
                    break;

                case "ProfilePage":
                    MainViewModel.GetInstance().Profile = new ProfileViewModel();
                    await App.Navigator.PushAsync(new ProfilePage());
                    break;

                case "SetupPage":
                    await App.Navigator.PushAsync(new SetupPage());
                    break;

                default:

                    #region Set default settings user (remember this device)

                    Settings.User = string.Empty;
                    Settings.IsRemember = false;
                    Settings.Token = string.Empty;
                    Settings.UserEmail = string.Empty;
                    Settings.UserPassword = string.Empty;

                    #endregion Set default settings user (remember this device)

                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
            }
        }

        #endregion Methods
    }
}