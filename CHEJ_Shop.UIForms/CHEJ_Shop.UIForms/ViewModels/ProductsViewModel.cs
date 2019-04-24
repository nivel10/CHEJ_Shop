namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ProductsViewModel : BaseViewModel
    {
        #region Attributes

        private readonly ApiService apiService;

        private readonly DialogService dialogService;

        #region Old Code

        //private ObservableCollection<Product> products; 

        #endregion Old Code

        private ObservableCollection<ProductItemViewModel> products;

        private List<Product> myProducts;

        private bool isRefreshing;

        #endregion Attributes

        #region Properties

        #region Old Code

        //public ObservableCollection<Product> Products
        //{
        //    get => this.products;
        //    set => this.SetValue(ref this.products, value);
        //} 

        #endregion Old Code

        public ObservableCollection<ProductItemViewModel> Products
        {
            get => this.products;
            set => this.SetValue(ref this.products, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        #endregion Properties

        #region Constructor

        public ProductsViewModel()
        {
            //  Load Objects
            this.apiService = new ApiService();
            this.dialogService = new DialogService();

            //  Load Products
            this.LoadProducts();
        }

        #endregion Constructor

        #region Methods

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            #region Old Code

            //var response = await this.apiService.GetListAsync<Product>(
            //    "http://chejconsultor.ddns.net:9002",
            //    "/api",
            //    "/Products"); 

            #endregion Old Code

            var response = await this.apiService.GetListAsync<Product>(
                MethodsHelper.GetUrlAPI,
                "/api",
                "/Products",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            //  Cast Data Products
            #region Old Code

            //var myProducts = (List<Product>)response.Result;
            //this.Products = new ObservableCollection<Product>(
            //    myProducts.OrderBy(p => p.Name)); 

            #endregion Old Code

            this.myProducts = (List<Product>)response.Result;
            this.RefreshProductList();
        }

        public void AddProductToList(
            Product _product)
        {
            this.myProducts.Add(_product);
            this.RefreshProductList();
        }

        public void UpdateProductInList(
            Product _product)
        {
            var previousProduct = this.myProducts
                .Where(p => p.Id == _product.Id)
                .FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.myProducts.Add(_product);
            this.RefreshProductList();
        }

        public void DeleteProductInList(
            int _productId)
        {
            var previousProduct = this.myProducts
                .Where(p => p.Id == _productId)
                .FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.RefreshProductList();
        }

        private void RefreshProductList()
        {
            this.Products = new ObservableCollection<ProductItemViewModel>(
                this.myProducts.Select(
                    p => new ProductItemViewModel
                    {
                        Id = p.Id,
                        ImageUrl = p.ImageUrl,
                        ImageFullPath = p.ImageFullPath,
                        IsAvailabe = p.IsAvailabe,
                        LastPurchase = p.LastPurchase,
                        LastSale = p.LastSale,
                        Name = p.Name,
                        Price = p.Price,
                        Stock = p.Stock,
                        User = p.User
                    })
                    .OrderBy(p => p.Name)
                    .ToList()
                    );
        }

        #endregion Methods
    }
}