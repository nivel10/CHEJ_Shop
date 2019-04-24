namespace CHEJ_Shop.Web.Controllers.API
{
    using CHEJ_Shop.Web.Data;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[Controller]")]
    public class CountriesController : Controller
    {
        #region Attributes

        private readonly ICountryRepository iCountryRepository;

        #endregion Attributes

        #region Constructor

        public CountriesController(
            ICountryRepository _iCountryRepository)
        {
            this.iCountryRepository = _iCountryRepository;
        }

        #endregion Constructor

        #region Methods

        [HttpGet]
        public IActionResult GetCountries()
        {
            return Ok(this.iCountryRepository.GetCountriesWithCities());
        }

        #endregion Methods
    }
}