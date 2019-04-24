namespace CHEJ_Shop.Common.Services
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiService
    {
        public async Task<Response> GetListAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };
                var url = $"{_servicePrefix}{_controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetListAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            string _tokenType,
            string _accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase),
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    _tokenType,
                    _accessToken);

                var url = $"{_servicePrefix}{_controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetTokenAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            TokenRequest _request)
        {
            try
            {
                var requestString = JsonConvert.SerializeObject(_request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                var url = $"{_servicePrefix}{_controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = token
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> PostAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            T _model,
            string _tokenType,
            string _accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(_model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    _tokenType,
                    _accessToken);
                var url = $"{_servicePrefix}{_controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> PutAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            int _id,
            T _model,
            string _tokenType,
            string _accessToken)
        {
            if (_servicePrefix == null)
            {
                throw new ArgumentNullException(nameof(_servicePrefix));
            }

            try
            {
                var request = JsonConvert.SerializeObject(_model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    _tokenType,
                    _accessToken);
                var url = $"{_servicePrefix}{_controller}/{_id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> DeleteAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            int _id,
            string _tokenType,
            string _accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    _tokenType,
                    _accessToken);
                var url = $"{_servicePrefix}{_controller}/{_id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Meyhod is ok...!!!",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> RegisterUserAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            NewUserRequest _newUserRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(_newUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                var url = $"{_servicePrefix}{_controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> RecoverPasswordAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            RecoverPasswordRequest _recoverPasswordRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(_recoverPasswordRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                var url = $"{_servicePrefix}{_controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetUserByEmailAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            string _email,
            string _tokenType,
            string _accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(
                    new RecoverPasswordRequest
                    {
                        Email = _email
                    });
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    _tokenType,
                    _accessToken);
                var url = $"{_servicePrefix}{_controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<User>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> PutAsync<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            T _model,
            string _tokenType,
            string _accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(_model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    _tokenType,
                    _accessToken);
                var url = $"{_servicePrefix}{_controller}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> ChangePasswordAsync(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            ChangePasswordRequest _changePasswordRequest,
            string _tokenType,
            string _accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(
                    _changePasswordRequest);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(_urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    _tokenType,
                    _accessToken);
                var url = $"{_servicePrefix}{_controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}