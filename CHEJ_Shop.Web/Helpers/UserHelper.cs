namespace CHEJ_Shop.Web.Helpers
{
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class UserHelper : IUserHelper
    {
        #region Attributes

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        #endregion Attributes

        #region Constructor

        public UserHelper(
            UserManager<User> _userManager,
            SignInManager<User> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.roleManager = _roleManager;
        }

        #endregion Constructor

        #region Methods

        #region Login / Logout

        public async Task<SignInResult> LoginAsync(
           LoginViewModel _model)
        {
            return await this.signInManager.PasswordSignInAsync(
                _model.UserName,
                _model.Password,
                _model.RememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        #endregion Login / Logout

        #region Password

        public async Task<IdentityResult> ChangePasswordAsync(
            User _user,
            string _oldPassword,
            string _newPassword)
        {
            return await this.userManager.ChangePasswordAsync(
                _user,
                _oldPassword,
                _newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(
            User _user)
        {
            return await this.userManager.UpdateAsync(_user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(
            User _user,
            string _password)
        {
            return await this.signInManager.CheckPasswordSignInAsync(
                _user,
                _password,
                false);
        }

        #endregion Password

        #region Roles

        public async Task CheckRoleAsync(string _roleName)
        {
            var roleExists =
                await this.roleManager.RoleExistsAsync(_roleName);
            if (!roleExists)
            {
                await this.roleManager.CreateAsync(new IdentityRole
                {
                    Name = _roleName
                });
            }
        }

        public async Task AddUserToRoleAsync(
            User _user,
            string _rolName)
        {
            await this.userManager.AddToRoleAsync(
                _user,
                _rolName);
        }

        public async Task<bool> IsUserInRoleAsync(
            User _user,
            string _roleName)
        {
            return await this.userManager.IsInRoleAsync(
                _user,
                _roleName);
        }

        #endregion Roles

        #region User

        public async Task<User> GetUserByEmailAsync(
            string _email)
        {
            return await this.userManager.FindByEmailAsync(_email);
        }

        public static bool UserIsAdmin(
            ClaimsPrincipal _user)
        {
            var roleAdmin = MethodsHelper.RoleAdmin;
            return _user.IsInRole(roleAdmin);
        }

        public async Task<IdentityResult> AddUserAsync(
            User _user,
            string _password)
        {
            return await this.userManager.CreateAsync(_user, _password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await this.userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await this.userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await this.userManager.FindByIdAsync(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await this.userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await this.userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await this.userManager.Users
                .Include(u => u.City)
                .OrderBy(u => u.FullName)            
                .ToListAsync();
        }

        public async Task RemoveUserFromRoleAsync(User user, string roleName)
        {
            await this.userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task DeleteUserAsync(User user)
        {
            await this.userManager.DeleteAsync(user);
        }

        #endregion User

        #endregion Methods
    }
}