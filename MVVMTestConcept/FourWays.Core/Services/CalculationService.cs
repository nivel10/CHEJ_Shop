namespace FourWays.Core.Services
{
    public class CalculationService : ICalculationService
    {
        public decimal TipAmount(
            decimal _subTotal,
            double _generosity)
        {
            return _subTotal * (decimal)(_generosity / 100.00);
        }
    }
}