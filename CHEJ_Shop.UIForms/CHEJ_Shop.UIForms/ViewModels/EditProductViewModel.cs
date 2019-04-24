namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using System;
    using System.Windows.Input;

    public class EditProductViewModel : BaseViewModel
    {
        #region Attributes

        private readonly ApiService apiService;

        private readonly DialogService dialogService;

        private bool isRunning;

        private bool isEnabled;

        #endregion Attributes

        #region Properties

        public Product Product { get; set; }

        public bool IsRunning { get => this.isRunning; set => this.SetValue(ref this.isRunning, value); }

        public bool IsEnabled { get => this.isEnabled; set => this.SetValue(ref this.isEnabled, value); }

        #region Commands

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand DeleteCommand => new RelayCommand(this.Delete);

        public ICommand BackCommand => new RelayCommand(this.Back);

        #endregion Commands

        #endregion Properties

        #region Constructor

        public EditProductViewModel(
           Product _product)
        {
            //  Get a instances of class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();

            this.Product = _product;

            //  Set status controls
            this.SetStatusControls(true, false);
        }

        #endregion Constructor

        #region Methods

        private void SetStatusControls(
           bool _isEnabled,
           bool _isRunning)
        {
            this.IsEnabled = _isEnabled;
            this.IsRunning = _isRunning;
        }

        private async void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Product.Name))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter a product name...!!!",
                        "Accept");
                    return;
                }

                if (this.Product.Price <= 0)
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "The price must be a number greather than zero...!!!",
                        "Accept");
                    return;
                }

                this.SetStatusControls(false, true);

                var response = await this.apiService.PutAsync(
                    MethodsHelper.GetUrlAPI,
                    "/api",
                    "/Products",
                    this.Product.Id,
                    this.Product,
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

                MainViewModel.GetInstance().Products.UpdateProductInList((Product)response.Result);
                this.Back();
            }
            catch (Exception)
            {
                this.SetStatusControls(true, false);
            }
        }

        private async void Delete()
        {
            try
            {
                var confirm = await this.dialogService.ShowMessage(
                    "Confirm",
                    "Are you sure to delete the product..?",
                    "Yes",
                    "No");
                if (!confirm)
                {
                    return;
                }

                this.SetStatusControls(false, true);

                var response = await this.apiService.DeleteAsync(
                    MethodsHelper.GetUrlAPI,
                   "/api",
                   "/Products",
                   this.Product.Id,
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

                MainViewModel.GetInstance().Products.DeleteProductInList(this.Product.Id);
                this.Back();
            }
            catch (Exception)
            {
                this.SetStatusControls(true, false);
            }
        }

        private async void Back()
        {
            await App.Navigator.PopAsync();
        }

        #endregion Methods
    }
}