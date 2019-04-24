using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHEJ_Shop.UIForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //  CHEJ - Navigator
            App.Navigator = this.Navigator;

            //  CHEJ - Navigator - Menu Auto Hide
            App.Master = this;
        }
    }
}