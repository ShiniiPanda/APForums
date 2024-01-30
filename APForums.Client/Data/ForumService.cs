using APForums.Client.Data.DTO;
using APForums.Client.Data.DTO.PagesDTO;
using APForums.Client.Data.Interfaces;
using APForums.Client.Data.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APForums.Client.Data
{
    public class ForumService : IForumService
    {
        HttpClient _httpClient;
        private readonly ILoginService _loginService;

        public ForumService(ILoginService loginService)
        {
            _httpClient = new HttpClient();
            _loginService = loginService;
        }

        public async Task<BasicHttpResponseWithData<Forum>> GetForum(int forumId)
        {
            if (Settings.authInfo == null)
            {
                return new()
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_FORUMS}/{forumId}");
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
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_FORUMS}/{forumId}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new()
                {
                    Data = JsonSerializer.Deserialize<Forum>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Forum was not found!"
                    };
                }
                return new()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request, please try again!"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<ForumWithRecentPost>>> GetSubscribedForums(int id)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<IEnumerable<ForumWithRecentPost>>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_FORUMS}/User/{id}/Subscriptions");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<IEnumerable<ForumWithRecentPost>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_FORUMS}/User/{id}/Subscriptions");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var resultList = JsonSerializer.Deserialize<IEnumerable<ForumWithRecentPost>>(result, new JsonSerializerOptions {  PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<IEnumerable<ForumWithRecentPost>>
                {
                    Data = resultList,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new BasicHttpResponseWithData<IEnumerable<ForumWithRecentPost>>
                    {
                        Status = HttpStatusCode.Forbidden,
                        Error = "User was not found!"
                    };
                }
                return new BasicHttpResponseWithData<IEnumerable<ForumWithRecentPost>>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request, please try again!"
                };
            }
        }

        public async Task<BasicHttpResponse> Subscribe(int userId, int forumId)
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
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_FORUMS}/User/{userId}/Subscriptions/{forumId}", new StringContent(""));
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
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_FORUMS}/User/{userId}/Subscriptions/{forumId}", new StringContent(""));

            }
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Error = "User has already subscribed to this forum!"
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Failed to subscribe to forum"
                };
            }
        }

        public async Task<BasicHttpResponse> Unsubscribe(int userId, int forumId)
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
            var response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_FORUMS}/User/{userId}/Subscriptions/{forumId}");
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
                response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_FORUMS}/User/{userId}/Subscriptions/{forumId}");

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
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Error = "User is not subscribed to this forum!"
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Failed to unsubscribe to forum, error " + response.StatusCode.ToString()
                };
            }
        }

        public async Task<BasicHttpResponseWithData<bool>> GetUserSubscriptionStatus(int userId, int forumId)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<bool>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_FORUMS}/User/{userId}/Subscriptions/{forumId}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<bool>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_FORUMS}/User/{userId}/Subscriptions/{forumId}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<bool>
                {
                    Data = bool.Parse(result),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<bool>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to fetch forum subscription status!"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<PaginatedList<ForumWithRecentPost>>> GetUserForums(int userId, int page, int type = 0, int size = 5, string search = "")
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<PaginatedList<ForumWithRecentPost>>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_FORUMS}/User/{userId}?type={type}&page={page}&size={size}&search={search}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<PaginatedList<ForumWithRecentPost>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_FORUMS}/User/{userId}?type={type}&page={page}&size={size}&search={search}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<PaginatedList<ForumWithRecentPost>>
                {
                    Data = JsonSerializer.Deserialize<PaginatedList<ForumWithRecentPost>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new BasicHttpResponseWithData<PaginatedList<ForumWithRecentPost>>
                    {
                        Status = HttpStatusCode.Forbidden,
                        Error = "User does not have permission to view those posts!"
                    };
                }
                return new BasicHttpResponseWithData<PaginatedList<ForumWithRecentPost>>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<int>> AddClubForum(int clubId, Forum forum)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<int>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(forum);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_FORUMS}/Club/{clubId}", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<int>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_FORUMS}/Club/{clubId}", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var forumIdString = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<int>
                {
                    Data = int.Parse(forumIdString),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new BasicHttpResponseWithData<int>
                    {
                        Status = HttpStatusCode.Forbidden,
                        Error = "User must be a commitee of this club to create a forum!"
                    };
                }
                return new BasicHttpResponseWithData<int>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request, please try again!"
                };
            }
        }

    }
}
