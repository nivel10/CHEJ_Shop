namespace CHEJ_Shop.Common.Interfaces
{
    using Common.Models;
    using System.Threading.Tasks;

    public interface IApiService
    {
        Task<Response> ChangePasswordAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            ChangePasswordRequest _changePasswordRequest,
            string _tokenType,
            string _accessToken);

        Task<Response> DeleteAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            int _id,
            string _tokenType,
            string _accessToken);

        Task<Response> GetListAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller);

        Task<Response> GetListAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            string _tokenType,
            string _accessToken);

        Task<Response> GetTokenAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            TokenRequest _request);

        Task<Response> GetUserByEmailAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            string _email,
            string _tokenType,
            string _accessToken);

        Task<Response> PostAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            T _model,
            string _tokenType,
            string _accessToken);

        Task<Response> PutAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            int _id, T _model,
            string _tokenType,
            string _accessToken);

        Task<Response> PutAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            T _model,
            string _tokenType,
            string _accessToken);

        Task<Response> RecoverPasswordAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            RecoverPasswordRequest _recoverPasswordRequest);

        Task<Response> RegisterUserAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            NewUserRequest _newUserRequest);
    }
}