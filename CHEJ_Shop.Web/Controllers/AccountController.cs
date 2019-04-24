namespace CHEJ_Shop.Web.Controllers
{
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Models;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        #region Attributes

        private readonly IUserHelper iUserHelper;
        private readonly IConfiguration iConfiguration;
        public readonly ICountryRepository iCountryRepository;
        private readonly IMailHelper iMailHelper;

        #endregion Attributes

        #region Constructor

        public AccountController(
            IUserHelper _iUserHelper,
            IConfiguration _iConfiguration,
            ICountryRepository _iCountryRepository,
            IMailHelper _iMailHelper)
        {
            this.iUserHelper = _iUserHelper;
            this.iConfiguration = _iConfiguration;
            this.iCountryRepository = _iCountryRepository;
            this.iMailHelper = _iMailHelper;
        }

        #endregion Constructor

        #region Methods Controller

        public ActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            LoginViewModel _model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.iUserHelper.LoginAsync(_model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login.");
            return this.View(_model);
        }

        public async Task<IActionResult> Logout()
        {
            await this.iUserHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Countries = this.iCountryRepository.GetComboCountries(),
                Cities = this.iCountryRepository.GetComboCities(0)
            };

            return this.View(model);
        }

        #region Old Code

        //[HttpPost]
        //public async Task<IActionResult> Register(
        //    RegisterNewUserViewModel _model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await this.iUserHelper.GetUserByEmailAsync(_model.Username);
        //        if (user == null)
        //        {
        //            var city = await this.iCountryRepository.GetCityAsync(
        //                _model.CityId);

        //            user = new User
        //            {
        //                FirstName = _model.FirstName,
        //                LastName = _model.LastName,
        //                Email = _model.Username,
        //                UserName = _model.Username,
        //                Address = _model.Address,
        //                PhoneNumber = _model.PhoneNumber,
        //                CityId = _model.CityId,
        //                City = city,
        //            };

        //            #region Create User

        //            var createUserResult = await this.iUserHelper.AddUserAsync(
        //                           user,
        //                           _model.Password);
        //            if (createUserResult != IdentityResult.Success)
        //            {
        //                this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
        //                return this.View(_model);
        //            }

        //            #endregion Create User

        //            #region Login User

        //            var loginViewModel = new LoginViewModel
        //            {
        //                Password = _model.Password,
        //                RememberMe = false,
        //                UserName = _model.Username,
        //            };

        //            var loginResult = await this.iUserHelper.LoginAsync(loginViewModel);

        //            if (loginResult.Succeeded)
        //            {
        //                return this.RedirectToAction("Index", "Home");
        //            }

        //            #endregion Login User

        //            this.ModelState.AddModelError(string.Empty, "The user couldn't be login.");
        //            return this.View(_model);
        //        }

        //        this.ModelState.AddModelError(string.Empty, "The username is already registered.");
        //    }

        //    return this.View(_model);
        //} 

        #endregion Old Code

        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterNewUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.iUserHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    var city = await this.iCountryRepository.GetCityAsync(model.CityId);

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        CityId = model.CityId,
                        City = city
                    };

                    var result = await this.iUserHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return this.View(model);
                    }

                    var myToken = await this.iUserHelper.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = this.Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new
                        {
                            userid = user.Id,
                            token = myToken
                        },
                        protocol: HttpContext.Request.Scheme);

                    var userDescription = $"{model.FirstName}, {model.LastName}";
                    var subjectEmail = "CHEJ Shop - Email confirmation";
                    var bodyEmail = "<h1>Email Confirmation</h1>";
                    bodyEmail += "To allow the user, ";
                    bodyEmail += $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>";

                    var response = await this.iMailHelper.SendMail(
                        model.Username,
                        userDescription,
                        subjectEmail,
                        bodyEmail);
                    if (response.IsSuccess)
                    {
                        this.ViewBag.Message = "The instructions to allow your user has been sent to email.";
                    }
                    else
                    {
                        this.ViewBag.Message = $"Error: {response.Message}";
                    }
                    return this.View(model);
                }

                model.Countries = this.iCountryRepository.GetComboCountries();
                model.Cities = this.iCountryRepository.GetComboCities(0);

                this.ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return this.View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            #region Old Code

            //var user = await this.iUserHelper.GetUserByEmailAsync(this.User.Identity.Name);
            //var model = new ChangeUserViewModel();
            //if (user != null)
            //{
            //    model.FirstName = user.FirstName;
            //    model.LastName = user.LastName;
            //}
            //return this.View(model); 

            #endregion Old Code

            var user = await this.iUserHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;

                var city = await this.iCountryRepository.GetCityAsync(user.CityId);
                if (city != null)
                {
                    var country = await this.iCountryRepository.GetCountryAsync(city);
                    if (country != null)
                    {
                        model.CountryId = country.Id;
                        model.Cities = this.iCountryRepository.GetComboCities(country.Id);
                        model.Countries = this.iCountryRepository.GetComboCountries();
                        model.CityId = user.CityId;
                    }
                }
            }

            model.Cities = this.iCountryRepository.GetComboCities(model.CountryId);
            model.Countries = this.iCountryRepository.GetComboCountries();

            return this.View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(
            ChangeUserViewModel _model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.iUserHelper.GetUserByEmailAsync(
                    this.User.Identity.Name);
                if (user != null)
                {
                    var city = await this.iCountryRepository.GetCityAsync(_model.CityId);

                    user.FirstName = _model.FirstName;
                    user.LastName = _model.LastName;
                    user.Address = _model.Address;
                    user.PhoneNumber = _model.PhoneNumber;
                    user.CityId = _model.CityId;
                    user.City = city;

                    var respose = await this.iUserHelper.UpdateUserAsync(user);
                    if (respose.Succeeded)
                    {
                        this.ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(_model);
        }

        public IActionResult ChangePassword()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordViewModel _model)
        {
            if (this.ModelState.IsValid)
            {
                var user =
                    await this.iUserHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result =
                        await this.iUserHelper.ChangePasswordAsync(user, _model.OldPassword, _model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(_model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(
            [FromBody] LoginViewModel _model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.iUserHelper.GetUserByEmailAsync(_model.UserName);
                if (user != null)
                {
                    var result = await this.iUserHelper.ValidatePasswordAsync(
                        user,
                        _model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.iConfiguration["Tokens:Key"]));
                        var credentials = new SigningCredentials(
                            key,
                            SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            this.iConfiguration["Tokens:Issuer"],
                            this.iConfiguration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }
            return this.BadRequest();
        }

        public IActionResult NotAuthorized()
        {
            return this.View();
        }

        public async Task<JsonResult> GetCitiesAsync(
            int countryId)
        {
            var country = await this.iCountryRepository.GetCountryWithCitiesAsync(
                countryId);
            return this.Json(country.Cities.OrderBy(c => c.Name));
        }

        public async Task<IActionResult> ConfirmEmail(
            string userId,
            string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return this.NotFound();
            }

            var user = await this.iUserHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            var result = await this.iUserHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return this.NotFound();
            }

            return View();
        }

        public IActionResult RecoverPassword()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(
            RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.iUserHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return this.View(model);
                }

                var myToken = await this.iUserHelper.GeneratePasswordResetTokenAsync(user);
                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken },
                    protocol: HttpContext.Request.Scheme);
                var sujectEmail = "Shop Password Reset";
                var bodyEmail = "<h1>Shop Password Reset</h1>";
                bodyEmail += "To reset the password click in this link:</br></br>";
                bodyEmail += $"<a href = \"{link}\">Reset Password</a>";

                var response = await this.iMailHelper.SendMail(
                    model.Email,
                    "User Recovery Password",
                sujectEmail,
                bodyEmail);

                if (response.IsSuccess)
                {
                    var message = "The instructions to recover your password has been sent to email.";
                    this.ViewBag.Message = $"{message}: {model.Email}";
                    model.Email = string.Empty;
                }
                else
                {
                    this.ViewBag.Message = $"Error: {response.Message}";
                }

                return this.View(model);
            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await this.iUserHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await this.iUserHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return this.View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await this.iUserHelper.GetAllUsersAsync();
            foreach (var user in users)
            {
                var myUser = await this.iUserHelper.GetUserByIdAsync(user.Id);
                if (myUser != null)
                {
                    user.IsAdmin = await this.iUserHelper.IsUserInRoleAsync(myUser, "Admin");
                }
            }

            return this.View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOff(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var roleAdmin = MethodsHelper.RoleAdmin;

            var user = await this.iUserHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //  await this.iUserHelper.RemoveUserFromRoleAsync(user, "Admin");
            await this.iUserHelper.RemoveUserFromRoleAsync(user, roleAdmin);

            return this.RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOn(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var roleAdmin = MethodsHelper.RoleAdmin;

            var user = await this.iUserHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //  await this.iUserHelper.AddUserToRoleAsync(user, "Admin");
            await this.iUserHelper.AddUserToRoleAsync(user, roleAdmin);

            return this.RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await this.iUserHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await this.iUserHelper.DeleteUserAsync(user);
            return this.RedirectToAction(nameof(Index));
        }

        #endregion Methods Controller
    }
}