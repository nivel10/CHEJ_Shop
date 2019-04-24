namespace CHEJ_Shop.UIForms.InfraStructure
{
    using ViewModels;

    public class InstanceLocator
    {
        #region Properties

        public MainViewModel Main { get; set; }

        #endregion Properties

        #region Constructor

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        } 

        #endregion Constructor
    }
}