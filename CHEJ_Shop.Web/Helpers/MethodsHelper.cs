namespace CHEJ_Shop.Web.Helpers
{
    using System;
    using System.Linq;
    using Web.Data;
    using Web.Data.Entities;

    public class MethodsHelper
    {
        #region App

        public static string GetPathImagesProducts => @"wwwroot\images\Products";

        public static object GetUrlImagesProducts => "~/images/Products/";

        #endregion App

        #region Company

        public static string GetUrlApi => "http://chejconsultor.ddns.net:9002";

        #endregion Company

        #region User

        public static string AdminFirstName => "CHEJ";

        public static string AdminLastName => "Consultor, C.A.";

        public static string AdminEmail => "chejconsultor@gmail.com";

        public static string AdminUserName => "chejconsultor@gmail.com";

        public static string AdminPhoneNumber => "+58 424 2703231";

        public static string AdminPassword => "Chej5654.";

        #endregion User

        #region Roles

        public static string RoleAdmin => "Admin";

        public static string RoleCustomer => "Customer";

        #endregion Roles

        #region Cities

        public static int GetCityIdByDescription(
           string _cityDescription,
           DataContext _context)
        {
            try
            {
                var city = _context.Cities
                    .Where(c => c.Name == _cityDescription)
                    .FirstOrDefault();

                return city.Id;
            }
            catch (Exception)
            {
                return _context.Cities.FirstOrDefault().Id;
            }
        }

        public static City GetCityByDescription(
            string _cityDescription,
            DataContext _context)
        {
            try
            {
                var city = _context.Cities
                    .Where(c => c.Name == _cityDescription)
                    .FirstOrDefault();

                return city;
            }
            catch (Exception)
            {
                return _context.Cities.FirstOrDefault();
            }
        }

        #endregion Cities
    }
}