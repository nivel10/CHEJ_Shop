namespace CHEJ_Shop.UIForms.ViewModels
{
    using Common.Models;
    using Common.Resources;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddProductViewModel : BaseViewModel
    {
        #region Attributes

        private readonly ApiService apiService;

        private readonly DialogService dialogService;

        //  private string image;

        private ImageSource imageSource;

        private MediaFile file;

        private bool isRunning;

        private bool isEnabled;

        #endregion Attributes

        #region Properties

        //  public string Image { get => this.image; set => this.SetValue(ref this.image, value); }

        public ImageSource ImageSource { get => this.imageSource; set => this.SetValue(ref this.imageSource, value); }

        public bool IsRunning { get => this.isRunning; set => this.SetValue(ref this.isRunning, value); }

        public bool IsEnabled { get => this.isEnabled; set => this.SetValue(ref this.isEnabled, value); }

        public string Name { get; set; }

        public string Price { get; set; }

        #region Commands

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand BackCommand => new RelayCommand(this.Back);

        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);

        #endregion Commands

        #endregion Properties

        #region Constructor

        public AddProductViewModel()
        {
            //  Get a instances of class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();

            //  Set status controls
            this.SetStatusControls(true, false);

            //  Set not image
            //  this.Image = MethodsHelper.GetUrlNotImage;
            this.ImageSource = MethodsHelper.GetUrlNotImage;
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
                if (string.IsNullOrEmpty(this.Name))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter a product name...!!!",
                        "Accept");
                    return;
                }

                if (string.IsNullOrEmpty(this.Price))
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "You must enter a product price...!!!",
                        "Accept");
                    return;
                }

                var price = decimal.Parse(this.Price);
                if (price <= 0)
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        "The price must be a number greather than zero...!!!",
                        "Accept");
                    return;
                }

                this.SetStatusControls(false, true);

                byte[] imageArray = null;
                if (this.file != null)
                {
                    imageArray = Common.Helpers.FilesHelper.ReadFully(
                        this.file.GetStream());
                }

                var product = new Product
                {
                    IsAvailabe = true,
                    Name = this.Name,
                    Price = price,
                    ImageArray = imageArray,
                    User = new User
                    {
                        Email = MainViewModel.GetInstance().UserEmail,
                    }
                };

                var url = MethodsHelper.GetUrlAPI;
                var response = await this.apiService.PostAsync(
                    url,
                    "/api",
                    "/Products",
                    product,
                    "bearer",
                    MainViewModel.GetInstance().Token.Token);

                if (!response.IsSuccess)
                {
                    this.SetStatusControls(true, false);

                    await this.dialogService.ShowMessage(
                        "Error",
                        response.Message,
                        "Accept");
                    return;
                }

                #region Old Code

                //var newProduct = (Product)response.Result;
                //MainViewModel.GetInstance().Products.Products.Add(newProduct); 

                #endregion Old Code

                //  MainViewModel.GetInstance().Products.Products.Add((Product)response.Result);
                MainViewModel.GetInstance().Products.AddProductToList((Product)response.Result);

                this.SetStatusControls(true, false);
                await App.Navigator.PopAsync();
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

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await this.dialogService.DisplayActionSheet(
                Resource.MsgWhereTakePicture,
                Resource.LblCancel,
                null,
                $"{Resource.LblFrom} {Resource.LblGallery}",
                $"{Resource.LblFrom} {Resource.LblCamera}");

            //  if (source == "Cancel")
            if (source == Resource.LblCancel)
            {
                this.file = null;
                return;
            }

            //  if (source == "From Camera")
            if (source == $"{Resource.LblFrom} {Resource.LblCamera}")
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Pictures",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        #endregion Methods
    }
}