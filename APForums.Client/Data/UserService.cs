using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APForums.Client.Data.Interfaces;
using System.Net;
using APForums.Client.Data.Structures;
using APForums.Client.Data.DTO;
using APForums.Client.Data.DTO.PagesDTO;

namespace APForums.Client.Data
{
    public class UserService : IUserService
    {
        private readonly ILoginService _loginService;
        HttpClient _httpClient;

        public UserService(ILoginService loginService)
        {
            _httpClient = new HttpClient();
            _loginService = loginService;
        }

#nullable enable
        public async Task<User?> GetUserInfoAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_USERS}/{userId}");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateCachedUserInfo(int id)
        {
            var user = await GetUserInfoAsync(id);
            if (user != null)
            {
                var userJsonStr = JsonSerializer.Serialize(user);
                SecureStorage.Default.Remove(nameof(Settings.userInfo));
                await SecureStorage.Default.SetAsync(nameof(Settings.userInfo), userJsonStr);
                Settings.userInfo = user;
                return true;
            } else
            {
                var userJsonStr = await SecureStorage.Default.GetAsync(nameof(Settings.userInfo));
                if (!string.IsNullOrWhiteSpace(userJsonStr))
                {
                    Settings.userInfo = JsonSerializer.Deserialize<User>(userJsonStr);
                    return true;
                } else
                {
                    return false;
                }
            }
        }

#nullable disable

        public async Task<BasicHttpResponseWithData<Profile>> GetUserProfile(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Profile/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<Profile>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Profile/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var profile = JsonSerializer.Deserialize<Profile>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<Profile>
                {
                    Data = profile,
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new BasicHttpResponseWithData<Profile>
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to find specified user, please try again!"
                };
            }
            else
            {
                return new BasicHttpResponseWithData<Profile>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to fetch profile."
                };
            }
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<BasicUser>>> GetUserConnections(int id, string type = "Followers")
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_USERS}/{id}/{type}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new()
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_USERS}/Home");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new()
                {
                    Data = JsonSerializer.Deserialize<IEnumerable<BasicUser>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to find specified user, please try again!"
                };
            }
            else
            {
                return new()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"Unable to fetch home page information!"
                };
            }
        }

        public async Task<BasicHttpResponse> FollowUser(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_USERS}/Follow/{id}", new StringContent(""));
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_USERS}/Follow/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to find specified user, please try again!"
                };
            }
            else
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to fetch profile."
                };
            }
        }

        public async Task<BasicHttpResponse> UnfollowUser(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_USERS}/Unfollow/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_USERS}/Unfollow/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to find specified user, please try again!"
                };
            }
            else
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponse> UpdatePicture(int id, string picture)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(picture);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType.MediaType = "application/json";
            var response = await _httpClient.PutAsync($"{ServicesApiRoutes.API_USERS}/{id}/Picture", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PutAsync($"{ServicesApiRoutes.API_USERS}/{id}/Picture", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to find specified user, please try again!"
                };
            }
            else 
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"AuthID: {Settings.authInfo.Id}, Content: {await content.ReadAsStringAsync()}, Status: {response.StatusCode.ToString()}"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<HomePage>> GetUserHomePage()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Home");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<HomePage>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Home");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<HomePage>
                {
                    Data = JsonSerializer.Deserialize<HomePage>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new BasicHttpResponseWithData<HomePage>
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to find specified user, please try again!"
                };
            }
            else
            {
                return new BasicHttpResponseWithData<HomePage>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"Unable to fetch home page information!"
                };
            }
        }

    }
}
