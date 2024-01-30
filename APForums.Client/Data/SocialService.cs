using APForums.Client.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APForums.Client.Data.DTO;
using APForums.Client.Data.Structures;

namespace APForums.Client.Data
{
    public class SocialService : ISocialService
    {
        private readonly ILoginService _loginService;
        HttpClient _httpClient;

        public SocialService(ILoginService loginService)
        {
            _loginService = loginService;
            _httpClient = new HttpClient();
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<SocialLink>>> GetUserSocials(int id)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<IEnumerable<SocialLink>>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_SOCIALS}/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<IEnumerable<SocialLink>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_SOCIALS}/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var socialsList = JsonSerializer.Deserialize<IEnumerable<SocialLink>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<IEnumerable<SocialLink>>
                {
                    Data = socialsList,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            { 
                return new BasicHttpResponseWithData<IEnumerable<SocialLink>>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponse> UpdateSocialLink(SocialLink social)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(social);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{ServicesApiRoutes.API_SOCIALS}", content);
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
                response = await _httpClient.PutAsync($"{ServicesApiRoutes.API_SOCIALS}", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            { 
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
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

        public async Task<BasicHttpResponseWithData<SocialLink>> AddSocialLink(SocialLink social)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<SocialLink>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(social);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_SOCIALS}", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<SocialLink>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_SOCIALS}", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<SocialLink>
                {
                    Data = JsonSerializer.Deserialize<SocialLink>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true}),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<SocialLink>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponse> DeleteSocialLink(int id)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_SOCIALS}/{id}");
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
                response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_SOCIALS}/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
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


    }
}
