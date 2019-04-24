namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Views;

    public class ProductItemViewModel : Product
    {
        #region Properties

        public ICommand SelectProductCommand => new RelayCommand(this.SelectProduct);

        #endregion Properties

        #region Methods

        private async void SelectProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel((Product)this);
            await App.Navigator.PushAsync(new EditProductPage());
        }  

        #endregion Methods
    }
}