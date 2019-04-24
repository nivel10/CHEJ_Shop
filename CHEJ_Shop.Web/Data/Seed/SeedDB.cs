namespace CHEJ_Shop.Web.Data.Seed
{
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class SeedDb
    {
        #region Attributes

        private readonly DataContext context;
        private readonly IUserHelper iUserHelper;
        private readonly Random random;

        #endregion Attributes

        #region Constructor

        public SeedDb(
            DataContext _context,
            IUserHelper _iUserHelper)
        {
            this.context = _context;
            this.iUserHelper = _iUserHelper;
            this.random = new Random();
        }

        #endregion Constructor

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            #region Create Roles

            await this.CheckRolesAsync();

            #endregion

            #region Create Cities

            if (!this.context.Countries.Any())
            {
                await this.AddCountriesAndCitiesAsync();

                #region Old Code

                //var cities = new List<City>();

                //#region Old Code

                ////cities.Add(new City { Name = "Medellín" });
                ////cities.Add(new City { Name = "Bogotá" });
                ////cities.Add(new City { Name = "Calí" });

                ////this.context.Countries.Add(new Country
                ////{
                ////    Cities = cities,
                ////    Name = "Colombia"
                ////});

                ////await this.context.SaveChangesAsync();

                ////cities.Clear();

                //#endregion Old Code

                //cities.Add(new City { Name = "Caracas", });
                //cities.Add(new City { Name = "Valencia", });
                //cities.Add(new City { Name = "Maracay", });
                //cities.Add(new City { Name = "Maracaibo", });

                //this.context.Countries.Add(new Country
                //{
                //    Cities = cities,
                //    Name = "Venezuela",
                //});

                //await this.context.SaveChangesAsync(); 

                #endregion Old Code
            }

            #endregion Create Cities

            #region Create user

            //  Customer Users
            await this.CheckUserAsync(
                "nikole.a.herrera.v@gmail.com",
                "123456",
                "Nikole A.",
                "Herrera V.",
                "(+58) 424 2703231",
                "Crta. Vieja Junquito - Caracas. Casa Nro 10. Callejon Bucaral.",
                "Caracas",
                MethodsHelper.RoleCustomer);

            await this.CheckUserAsync(
                "brad@gmail.com",
                "123456",
                "Brad",
                "Pit",
                "(+1) 23 23454321",
                "Calle Luna, Calle Sol",
                "Miami",
                MethodsHelper.RoleCustomer);

            await this.CheckUserAsync(
                "angelina@gmail.com",
                "123456",
                "Angelina",
                "Jolie",
                 "(+1) 23 23454321",
                "Calle Luna, Calle Sol - 02",
                "Cartagena",
                MethodsHelper.RoleCustomer);

            //  Admin Users
            var userdmin = await this.CheckUserAsync(
                MethodsHelper.AdminEmail,
                MethodsHelper.AdminPassword,
                MethodsHelper.AdminFirstName,
                MethodsHelper.AdminLastName,
                MethodsHelper.AdminPhoneNumber,
                "Urb. El Paraiso. La Paz",
                "Caracas",
                MethodsHelper.RoleAdmin);

            #endregion Create user

            #region Create product

            if (!this.context.Products.Any())
            {
                this.AddProduct("iPhone X", userdmin);
                this.AddProduct("Magic Mouse", userdmin);
                this.AddProduct("iWatch Series 4", userdmin);
                this.AddProduct("iPad 10", userdmin);
                this.AddProduct("iPhone 7S", userdmin);
                this.AddProduct("MacBook Pro", userdmin);
                this.AddProduct("MacBook Air", userdmin);
                this.AddProduct("AirPods 2", userdmin);
                this.AddProduct("AirPods", userdmin);
                this.AddProduct("Blackmagic eGPU Pro", userdmin);
                this.AddProduct("iPad Pro", userdmin);
                this.AddProduct("iMac", userdmin);
                this.AddProduct("Mac Mini", userdmin);
                this.AddProduct("Magic Trackpad 2", userdmin);
                this.AddProduct("USB C Multiport", userdmin);
                this.AddProduct("Wireless Charging Pad", userdmin);

                await this.context.SaveChangesAsync();
            }

            #endregion Create product
        }

        #region Methods

        //  private void AddProduct(string name)
        private void AddProduct(string name, User _user)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = this.random.Next(100),
                IsAvailabe = true,
                ImageUrl = string.Empty,
                Stock = this.random.Next(100),
                User = _user,
            });
        }

        private async Task AddCountriesAndCitiesAsync()
        {
            this.AddCountry(
                "Colombia",
                new string[] {
                    "Medellín",
                    "Bogota",
                    "Calí",
                    "Barranquilla",
                    "Bucaramanga",
                    "Cartagena",
                    "Pereira"});
            this.AddCountry(
                "Argentina",
                new string[] {
                    "Córdoba",
                    "Buenos Aires",
                    "Rosario",
                    "Tandil",
                    "Salta",
                    "Mendoza" });
            this.AddCountry(
                "Estados Unidos",
                new string[] {
                    "New York",
                    "Los Ángeles",
                    "Chicago",
                    "Washington",
                    "San Francisco",
                    "Miami",
                    "Boston" });
            this.AddCountry(
                "Ecuador",
                new string[] {
                    "Quito",
                    "Guayaquil",
                    "Ambato",
                    "Manta",
                    "Loja",
                    "Santo" });
            this.AddCountry(
                "Peru",
                new string[] {
                    "Lima",
                    "Arequipa",
                    "Cusco",
                    "Trujillo",
                    "Chiclayo",
                    "Iquitos" });
            this.AddCountry(
                "Chile",
                new string[] {
                    "Santiago",
                    "Valdivia",
                    "Concepcion",
                    "Puerto Montt",
                    "Temucos",
                    "La Sirena" });
            this.AddCountry(
                "Uruguay",
                new string[] {
                    "Montevideo",
                    "Punta del Este",
                    "Colonia del Sacramento",
                    "Las Piedras" });
            this.AddCountry(
                "Bolivia",
                new string[] {
                    "La Paz",
                    "Sucre",
                    "Potosi",
                    "Cochabamba" });
            this.AddCountry(
                "Venezuela",
                new string[] {
                    "Caracas",
                    "Valencia",
                    "Maracaibo",
                    "Ciudad Bolivar",
                    "Maracay",
                    "Barquisimeto" });
            this.AddCountry(
                "Paraguay",
                new string[] {
                    "Asunción",
                    "Ciudad del Este",
                    "Encarnación",
                    "San  Lorenzo",
                    "Luque",
                    "Areguá" });
            this.AddCountry(
                "Brasil",
                new string[] {
                    "Rio de Janeiro",
                    "São Paulo",
                    "Salvador",
                    "Porto Alegre",
                    "Curitiba",
                    "Recife",
                    "Belo Horizonte",
                    "Fortaleza" });
            this.AddCountry(
                "Panamá",
                new string[] {
                    "Chitré",
                    "Santiago",
                    "La Arena",
                    "Agua Dulce",
                    "Monagrillo",
                    "Ciudad de Panamá",
                    "Colón",
                    "Los Santos" });
            this.AddCountry(
                "México",
                new string[] {
                    "Guadalajara",
                    "Ciudad de México",
                    "Monterrey",
                    "Ciudad Obregón",
                    "Hermosillo",
                    "La Paz",
                    "Culiacán",
                    "Los Mochis" });

            await this.context.SaveChangesAsync();
        }

        private void AddCountry(string country, string[] cities)
        {
            var theCities = cities.Select(c => new City { Name = c }).ToList();
            this.context.Countries.Add(new Country
            {
                Cities = theCities,
                Name = country
            });
        }

        private async Task CheckRolesAsync()
        {
            await this.iUserHelper.CheckRoleAsync(MethodsHelper.RoleAdmin);
            await this.iUserHelper.CheckRoleAsync(MethodsHelper.RoleCustomer);
        }

        private async Task<User> CheckUserAsync(
            string _userName,
            string _password,
            string _firtsName,
            string _lastName,
            string _phoneNumber,
            string _address,
            string _cityDescription,
            string _role)
        {
            #region Admin User / Role

            var user = await this.iUserHelper.GetUserByEmailAsync(
                _userName);

            if (user == null)
            {
                user = new User
                {
                    FirstName = _firtsName,
                    LastName = _lastName,
                    Email = _userName,
                    UserName = _userName,
                    PhoneNumber = _phoneNumber,
                    Address = _address,
                    CityId = MethodsHelper.GetCityIdByDescription(_cityDescription, context),
                    City = MethodsHelper.GetCityByDescription(_cityDescription, context),
                };

                var result = await this.iUserHelper.AddUserAsync(
                    user,
                    _password);
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await this.iUserHelper.AddUserToRoleAsync(
                    user,
                    _role);
            }

            //  var chanePassword = await this.iUserHelper.ChangePasswordAsync(user, "123456", "Chej5654.");

            var isInRole = await this.iUserHelper.IsUserInRoleAsync(
                user,
                _role);
            if (!isInRole)
            {
                await this.iUserHelper.AddUserToRoleAsync(
                    user,
                    _role);
            }

            #region Confirm Email

            var token = await this.iUserHelper.GenerateEmailConfirmationTokenAsync(user);
            await this.iUserHelper.ConfirmEmailAsync(user, token);

            #endregion Confirm Email

            #endregion Admin User / Role

            return user;
        }

        #endregion Methods
    }
}