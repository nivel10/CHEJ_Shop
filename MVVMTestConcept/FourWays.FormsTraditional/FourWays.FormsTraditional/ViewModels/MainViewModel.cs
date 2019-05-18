namespace FourWays.FormsTraditional.ViewModels
{
    using Core.Services;

    public class MainViewModel : BaseViewModel
    {
        #region Attributes

        private ICalculationService calculationService;
        private decimal amount;
        private double generosity;
        private decimal tip; 

        #endregion Attributes

        #region Properties

        public decimal Amount
        {
            get => this.amount;
            set
            {
                this.SetValue(ref this.amount, value);
                this.Recalculate();
            }
        }

        public double Generosity
        {
            get => this.generosity;
            set
            {
                this.SetValue(ref this.generosity, value);
                this.Recalculate();
            }
        }

        public decimal Tip
        {
            get => this.tip;
            set => this.SetValue(ref this.tip, value);
        } 

        #endregion Properties

        public MainViewModel()
        {
            this.calculationService = new CalculationService();
            this.Amount = 100;
            this.Generosity = 10;
        }

        private void Recalculate()
        {
            var tipCalculate =

            this.Tip = this.calculationService.TipAmount(
                this.Amount, 
                this.Generosity);
        }
    }
}
