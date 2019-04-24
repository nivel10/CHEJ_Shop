namespace CHEJ_Shop.Common.Services
{
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class DialogService
    {
        public async Task ShowMessage(
            string _title,
            string _message,
            string _button)
        {
            await Application.Current.MainPage.DisplayAlert(
                _title,
                _message,
                _button);
        }

        public async Task<bool> ShowMessage(
            string _title,
            string _message,
            string _buttonYes,
            string _buttonNo)
        {
            return await Application.Current.MainPage.DisplayAlert(
                _title,
                _message,
                _buttonYes,
                _buttonNo);
        }

        public async Task<string> DisplayActionSheet(
            string _title,
            string _cancel,
            string _destruction,
            string _option1,
            string _optionn2)
        {
            return await Application.Current.MainPage.DisplayActionSheet(
                _title,
                _cancel,
                _destruction,
                _option1,
                _optionn2);
        }
    }
}