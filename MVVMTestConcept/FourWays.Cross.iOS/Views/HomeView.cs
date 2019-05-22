namespace FourWays.Cross.iOS.Views
{
    using Core.ViewModels;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Platforms.Ios.Views;

    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class ViewController : MvxViewController<TipViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<ViewController, TipViewModel>();
            //  set.Bind(this.AmountText).To(vm => vm.SubTotal);
            //  set.Bind(this.GenerositySlider).To(vm => vm.Generosity);
            //  set.Bind(this.TipLabel).To(vm => vm.Tip);
            //  set.Apply();
        }
    }
}