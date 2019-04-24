namespace CHEJ_Shop.Web.Controllers.API
{
    using Common.Models;
    using Data;
    using Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    public class AccountController : Controller
    {
        private readonly IUserHelper iUserHelper;
        private readonly ICountryRepository iCountryRepository;
        private readonly IMailHelper iMailHelper;

        public AccountController(
            IUserHelper _iUserHelper,
            ICountryRepository _iCountryRepository,
            IMailHelper _iMailHelper)
        {
            this.iUserHelper = _iUserHelper;
            this.iCountryRepository = _iCountryRepository;
            this.iMailHelper = _iMailHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(
            [FromBody] NewUserRequest _request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.iUserHelper.GetUserByEmailAsync(_request.Email);
            if (user != null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "This email is already registered."
                });
            }

            var city = await this.iCountryRepository.GetCityAsync(_request.CityId);
            if (city == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "City don't exists."
                });
            }

            user = new Data.Entities.User
            {
                FirstName = _request.FirstName,
                LastName = _request.LastName,
                Email = _request.Email,
                UserName = _request.Email,
                Address = _request.Address,
                PhoneNumber = _request.Phone,
                CityId = _request.CityId,
                City = city
            };

            var result = await this.iUserHelper.AddUserAsync(user, _request.Password);
            if (result != IdentityResult.Success)
            {
                return this.BadRequest(result.Errors.FirstOrDefault().Description);
            }

            var myToken = await this.iUserHelper.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = this.Url.Action(
                "ConfirmEmail",
                "Account",
                new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

            var emailSubject = "Email confirmation";
            var emailBody = $"<h1>Email Confirmation</h1>";
            emailBody += "To allow the user, ";
            emailBody += $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>";
            var emailToDescription = $"{_request.FirstName}, {_request.LastName}";

            var response = await this.iMailHelper.SendMail(
                _request.Email,
                emailToDescription,
                emailSubject,
                emailBody);

            if (!response.IsSuccess)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = $"Error: {response.Message}",
                });
            }

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "A Confirmation email was sent. Plese confirm your account and log into the App."
            });
        }

        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(
            [FromBody] RecoverPasswordRequest _request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.iUserHelper.GetUserByEmailAsync(_request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "This email is not assigned to any user."
                });
            }

            var myToken = await this.iUserHelper.GeneratePasswordResetTokenAsync(user);
            var link = this.Url.Action(
                "ResetPassword",
                "Account",
                new { token = myToken }, protocol: HttpContext.Request.Scheme);

            var emailTo = _request.Email;
            var emailSubject = "Password Reset";
            var emailBody = "<h1>Recover Password</h1>";
            emailBody += "To reset the password click in this link:</br></br>";
            emailBody += $"<a href = \"{link}\">Reset Password</a>";

            var response = await iMailHelper.SendMail(
                emailTo,
                string.Empty,
                emailSubject,
                emailBody);

            if (!response.IsSuccess)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = $"Error: {response.Message}...!!!",
                });
            }

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "An email with instructions to change the password was sent...!!!"
            });
        }

        [HttpPost]
        [Route("GetUserByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserByEmail(
            [FromBody] RecoverPasswordRequest _request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.iUserHelper.GetUserByEmailAsync(_request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "User don't exists."
                });
            }

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> PutUser(
            [FromBody] User _user)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var userEntity = await this.iUserHelper.GetUserByEmailAsync(_user.Email);
            if (userEntity == null)
            {
                return this.BadRequest($"User: {_user.Email} not found...!!!");
            }

            var city = await this.iCountryRepository.GetCityAsync(_user.CityId);
            if (city != null)
            {
                userEntity.City = city;
            }

            userEntity.FirstName = _user.FirstName;
            userEntity.LastName = _user.LastName;
            userEntity.CityId = _user.CityId;
            userEntity.Address = _user.Address;
            userEntity.PhoneNumber = _user.PhoneNumber;

            var respose = await this.iUserHelper.UpdateUserAsync(userEntity);
            if (!respose.Succeeded)
            {
                return this.BadRequest($"Error: {respose.Errors.FirstOrDefault().Description}");
            }

            var updatedUser = await this.iUserHelper.GetUserByEmailAsync(_user.Email);
            return Ok(updatedUser);
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordRequest _request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.iUserHelper.GetUserByEmailAsync(_request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = $"This email: {_request.Email} is not assigned to any user....!!!",
                });
            }

            var result = await this.iUserHelper.ChangePasswordAsync(
                user,
                _request.OldPassword,
                _request.NewPassword);
            if (!result.Succeeded)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault().Description
                });
            }

            return this.Ok(new Response
            {
                IsSuccess = true,
                Message = "The password was changed succesfully!"
            });
        }
    }
}