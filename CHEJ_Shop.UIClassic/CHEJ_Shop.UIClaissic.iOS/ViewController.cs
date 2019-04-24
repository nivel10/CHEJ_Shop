namespace CHEJ_Shop.UIClaissic.iOS
{
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using UIKit;

    public partial class ViewController : UIViewController
    {
        #region Attributes

        private ApiService apiService;

        #endregion Attributes

        #region Constructor

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //  CHEJ - Set Intial Data
            this.SetInitialData();
        }

        #endregion Constructor

        #region Methods

        private void SetInitialData()
        {
            //  Set new instance
            this.apiService = new ApiService();

            //  Temporal set
            this.emailText.Text = "nikole.a.herrera.v@gmail.com";
            this.passwordText.SecureTextEntry = true;
            this.passwordText.Text = "123456";

            //  Set Status Controls
            this.SetStatusControls(true, false);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void LoginButton_TouchUpInside(
            UIButton _sender)
        {
            if (string.IsNullOrEmpty(this.emailText.Text))
            {
                var alertWarning = UIAlertController.Create(
                    "Warning",
                    "You must enter a vald email...!!!",
                    UIAlertControllerStyle.Alert);
                alertWarning.AddAction(
                    UIAlertAction.Create(
                        "Accept",
                        UIAlertActionStyle.Default,
                        null));
                this.PresentViewController(alertWarning, true, null);
                return;
            }

            if (string.IsNullOrEmpty(this.passwordText.Text))
            {
                var alertWarning = UIAlertController.Create(
                    "Warning",
                    "You must enter a valid password...!!!",
                    UIAlertControllerStyle.Alert);
                alertWarning.AddAction(
                    UIAlertAction.Create(
                        "Accept",
                        UIAlertActionStyle.Default,
                        null));
                this.PresentViewController(alertWarning, true, null);
                return;
            }

            #region Old Code

            //var response = await this.DoLogin();

            //var alertOk = UIAlertController.Create(
            //    response.IsSuccess ? "Information" : "Error",
            //    response.Message,
            //    UIAlertControllerStyle.Alert);
            //alertOk.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
            //this.PresentViewController(alertOk, true, null); 

            #endregion Old Code

            this.DoLogin();
        }

        private async void DoLogin()
        {
            try
            {
                #region Get Data Token

                //  Set Status Controls
                this.SetStatusControls(false, true);

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

                //  Set Status Controls
                this.SetStatusControls(true, false);

                if (!response.IsSuccess)
                {
                    var alertError = UIAlertController.Create(
                        "Error",
                        response.Message,
                        UIAlertControllerStyle.Alert);
                    alertError.AddAction(
                        UIAlertAction.Create(
                            "Accept",
                            UIAlertActionStyle.Default,
                            null));
                    this.PresentViewController(alertError, true, null);
                    return;
                }

                var token = (TokenResponse)response.Result;

                #endregion Get Data Token

                #region Old Code

                //var alertOk = UIAlertController.Create(
                //    "Information",
                //    "Everything is ok dude...!!!",
                //    UIAlertControllerStyle.Alert);
                //alertOk.AddAction(
                //    UIAlertAction.Create(
                //        "Accept",
                //        UIAlertActionStyle.Default,
                //        null));
                //this.PresentViewController(alertOk, true, null); 

                #endregion Old Code

                #region Get Data Products

                //  Set Status Controls
                this.SetStatusControls(false, true);

                response = await apiService.GetListAsync<Product>(
                    MethodsHelper.GetUrlAPI,
                    "/api",
                    "/Products",
                    "bearer", token.Token);

                //  Set Status Controls
                this.SetStatusControls(true, false);

                if (!response.IsSuccess)
                {
                    var alertError = UIAlertController.Create(
                        "Error",
                        response.Message,
                        UIAlertControllerStyle.Alert);
                    alertError.AddAction(
                        UIAlertAction.Create(
                            "Accept",
                            UIAlertActionStyle.Default,
                            null));
                    this.PresentViewController(alertError, true, null);
                    return;
                }

                #endregion Get Data Products

                #region Set Settings

                Settings.UserEmail = this.emailText.Text;
                Settings.Token = JsonConvert.SerializeObject(token);
                Settings.Products = JsonConvert.SerializeObject((List<Product>)response.Result);

                #endregion Set Settings

                #region Navigation

                var board = UIStoryboard.FromName(
                            "Main",
                            null);
                var productsViewController = board.InstantiateViewController("ProductsViewController");
                productsViewController.Title = "Products";
                this.NavigationController.PushViewController(
                    productsViewController,
                    true);

                #endregion Navigation
            }
            catch (Exception ex)
            {
                var alertError = UIAlertController.Create(
                       "Error",
                       ex.Message,
                       UIAlertControllerStyle.Alert);
                alertError.AddAction(
                    UIAlertAction.Create(
                        "Accept",
                        UIAlertActionStyle.Default,
                        null));
                this.PresentViewController(alertError, true, null);
                return;
            }
        }

        private void SetStatusControls(
            bool _isEnabled,
            bool _isRunning)
        {
            this.emailText.Enabled = _isEnabled;
            this.passwordText.Enabled = _isEnabled;
            this.loginButton.Enabled = _isEnabled;

            if (_isRunning)
            {
                this.activityIndicator.StartAnimating();
            }
            else
            {
                this.activityIndicator.StopAnimating();
            }
        }

        #endregion Methods
    }
}