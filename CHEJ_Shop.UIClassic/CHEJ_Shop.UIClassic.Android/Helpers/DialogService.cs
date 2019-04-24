namespace CHEJ_Shop.UIClassic.Android.Helpers
{
    using global::Android.App;
    using global::Android.Content;

    public class DialogService
    {
        public static void ShowMessage(
            Context _context,
            string _title,
            string _message,
            string _button)
        {
            new AlertDialog.Builder(_context)
                .SetTitle(_title)
                .SetMessage(_message)
                .SetPositiveButton(_button, (sent, args) => { })
                .SetCancelable(false)
                .Show();
        }
    }
}