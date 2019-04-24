namespace CHEJ_Shop.UIClassic.Android.Activities
{
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using global::Android.App;
    using global::Android.Content;
    using global::Android.OS;
    using global::Android.Support.V7.App;
    using global::Android.Views;
    using global::Android.Widget;
    using Newtonsoft.Json;
    using System;

    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme",
        MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        #region Attributes

        private ApiService apiService;
        private DialogService dialogService;

        #region Controls

        private EditText emailText;
        private EditText passwordText;
        private ProgressBar activityIndicatorProgressBar;
        private Button loginButton;

        #endregion Controls

        #endregion Attributes

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            //  CHEJ - View
            this.SetContentView(Resource.Layout.LoginPage);
            this.FindViews();
            this.HandleEvents();
            this.SetInitialData();
        }

        #region Methods

        private void FindViews()
        {
            this.emailText = this.FindViewById<EditText>(Resource.Id.emailText);
            this.passwordText = this.FindViewById<EditText>(Resource.Id.passwordText);
            this.activityIndicatorProgressBar = this.FindViewById<ProgressBar>(Resource.Id.activityIndicatorProgressBar);
            this.loginButton = this.FindViewById<Button>(Resource.Id.loginButton);
        }

        private void HandleEvents()
        {
            this.loginButton.Click += LoginButton_Click;
        }

        private async void LoginButton_Click(
            object _sender,
            EventArgs _e)
        {
            if (string.IsNullOrEmpty(this.emailText.Text))
            {
                Helpers.DialogService.ShowMessage(
                    this,
                    "Error",
                    "You must enter an email...!!!",
                    "Accept");

                //this.dialogService.ShowMessage(
                //    this,
                //    "Error",
                //    "You must enter an email...!!!",
                //    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.passwordText.Text))
            {
                Helpers.DialogService.ShowMessage(
                    this,
                    "Error",
                    "You must enter an password...!!!",
                    "Accept");

                //this.dialogService.ShowMessage(
                //    this,
                //    "Error",
                //    "You must enter an password...!!!",
                //    "Accept");
                return;
            }

            this.SetStatusControls(false, ViewStates.Visible);

            var tokenRequest = new TokenRequest
            {
                Password = this.passwordText.Text,
                Username = this.emailText.Text,
            };

            var response = await this.apiService.GetTokenAsync(
                MethodsHelper.GetUrlAPI,
                "/Account",
                "/CreateToken",
                tokenRequest);

            this.SetStatusControls(true, ViewStates.Invisible);

            if (!response.IsSuccess)
            {
                Helpers.DialogService.ShowMessage(
                    this,
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            //Helpers.DialogService.ShowMessage(
            //        this,
            //        "Information",
            //        "Everything ok...!!!",
            //        "Accept");
            //return;

            var tokenResponse = (TokenResponse)response.Result;
            var intent = new Intent(this, typeof(ProductsActivity));
            intent.PutExtra("tokenResponse", JsonConvert.SerializeObject(tokenResponse));
            intent.PutExtra("email", this.emailText.Text);
            this.StartActivity(intent);
        }

        private void SetInitialData()
        {
            this.apiService = new ApiService();
            this.dialogService = new Common.Services.DialogService();

            //  CHEJ - Temp
            this.emailText.Text = "nikole.a.herrera.v@gmail.com";
            this.passwordText.Text = "123456";

            //  Set Status Controls
            this.SetStatusControls(true, ViewStates.Invisible);
        }

        private void SetStatusControls(
            bool _isEnabled,
            ViewStates _viewsStates)
        {
            this.emailText.Enabled = _isEnabled;
            this.passwordText.Enabled = _isEnabled;
            this.loginButton.Enabled = _isEnabled;
            this.activityIndicatorProgressBar.Visibility = _viewsStates;
        }

        #endregion Methods
    }
}