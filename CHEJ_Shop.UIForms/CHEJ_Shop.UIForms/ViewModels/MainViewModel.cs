namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Views;

    public class MainViewModel
    {
        private static MainViewModel instance;

        #region Properties

        public LoginViewModel Login { get; set; }

        public ProductsViewModel Products { get; set; }

        public TokenResponse Token { get; set; }

        public AddProductViewModel AddProduct { get; set; }

        public EditProductViewModel EditProduct { get; set; }

        public RegisterViewModel Register { get; set; }

        public RememberPasswordViewModel RememberPassword { get; set; }

        public ProfileViewModel Profile { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public User User { get; set; }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        #region Commands

        public ICommand AddProductCommand => new RelayCommand(this.GoAddProduct);

        #endregion Commands

        #endregion Properties

        #region Constructor

        public MainViewModel()
        {
            //  this.Login = new LoginViewModel();

            //  Set Instance MainViewModel
            instance = this;

            //  Load menu
            this.LoadMenu();
        }

        #endregion Constructor

        #region Methods

        //  Singleton
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        private void LoadMenu()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_info",
                    PageName = "AboutPage",
                    Title = "About",
                },

                new Menu
                {
                    Icon = "ic_person",
                    PageName = "ProfilePage",
                    Title = "Modify User",
                },

                new Menu
                {
                    Icon = "ic_phonelink_setup",
                    PageName = "SetupPage",
                    Title = "Setup"
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = "Close session"
                },
            };

            this.Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                }));
        }

        private async void GoAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await App.Navigator.PushAsync(new AddProductPage());
        }

        #endregion Methods
    }
}