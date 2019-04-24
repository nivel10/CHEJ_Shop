namespace CHEJ_Shop.UIClassic.Android.Activities
{
    using Adapters;
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using global::Android.App;
    using global::Android.OS;
    using global::Android.Support.V7.App;
    using global::Android.Widget;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme",
        MainLauncher = false)]
    public class ProductsActivity : AppCompatActivity
    {
        #region Attributes

        private TokenResponse tokenResponse;
        private string email;
        private ApiService apiService;
        private ListView productsListView;

        #endregion Attributes

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.ProductPage);

            this.SetInitialData();
            this.LoadProducts();
        }

        #region Methods

        private void SetInitialData()
        {
            this.productsListView = this.FindViewById<ListView>(Resource.Id.productsListView);
            this.email = Intent.Extras.GetString("email");
            var tokenString = Intent.Extras.GetString("tokenResponse");
            this.tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(tokenString);
            this.apiService = new ApiService();
        }

        private async void LoadProducts()
        {
            var response = await this.apiService.GetListAsync<Product>(
                MethodsHelper.GetUrlAPI,
                "/api",
                "/Products",
                "bearer",
                this.tokenResponse.Token);

            if (!response.IsSuccess)
            {
                Helpers.DialogService.ShowMessage(
                    this,
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var products = (List<Product>)response.Result;
            this.productsListView.Adapter = new ProductsListAdapter(this, products);
            this.productsListView.FastScrollEnabled = true;
        }

        #endregion Methods
    }
}