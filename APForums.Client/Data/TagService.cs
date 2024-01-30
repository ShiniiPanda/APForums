using APForums.Client.Data.DTO;
using APForums.Client.Data.DTO.PagesDTO;
using APForums.Client.Data.Interfaces;
using APForums.Client.Data.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APForums.Client.Data
{
    public class TagService : ITagService
    {

        private readonly ILoginService _loginService;
        HttpClient _httpClient;

        public TagService(ILoginService loginService)
        {
            _httpClient = new HttpClient();
            _loginService = loginService;
        }

        public async Task<BasicHttpResponseWithData<SocialProfileTag>> GetSocialProfileTag(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_TAGS}/Social/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<SocialProfileTag>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_TAGS}/Social/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tag = JsonSerializer.Deserialize<SocialProfileTag>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<SocialProfileTag>
                {
                    Data = tag,
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new BasicHttpResponseWithData<SocialProfileTag>
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to find specified profile tag, please try again!"
                };
            }
            else
            {
                return new BasicHttpResponseWithData<SocialProfileTag>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to fetch profile tag."
                };
            }
        }

        public async Task<BasicHttpResponse> RemoveProfileTag(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_TAGS}/User/Remove/{id}");
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
                response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_TAGS}/User/Remove/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
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
                    Error = "Unable to find specified profile tag, please try again!"
                };
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.InternalServerError,
                    Error = "User does not have this profile tag to begin with!"
                };
            }
            else
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to fetch profile tag."
                };
            }
        }

        public async Task<BasicHttpResponse> AddProfileTag(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_TAGS}/User/Add/{id}", new StringContent(""));
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
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_TAGS}/User/Add/{id}", new StringContent(""));

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
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
                    Error = "Unable to find specified profile tag, please try again!"
                };
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.InternalServerError,
                    Error = "User already has this profile tag!"
                };
            }
            else
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to fetch profile tag."
                };
            }
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<ProfileTag>>> GetProfileTags(int id)
        {
            IEnumerable<ProfileTag> tags = new List<ProfileTag>();
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_TAGS}/User/{id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                tags = JsonSerializer.Deserialize<IEnumerable<ProfileTag>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<IEnumerable<ProfileTag>>
                {
                    Data = tags,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<IEnumerable<ProfileTag>>
                {
                    Data = tags,
                    Status = HttpStatusCode.NotFound,
                };
            }
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<ProfileTag>>> GetAllProfileTags()
        {
            IEnumerable<ProfileTag> tags = new List<ProfileTag>();
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_TAGS}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                tags = JsonSerializer.Deserialize<IEnumerable<ProfileTag>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<IEnumerable<ProfileTag>>
                {
                    Data = tags,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<IEnumerable<ProfileTag>>
                {
                    Data = tags,
                    Status = HttpStatusCode.NotFound,
                };
            }
        }

    }
}
