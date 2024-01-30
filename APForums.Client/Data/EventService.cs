using APForums.Client.Data.DTO;
using APForums.Client.Data.DTO.PagesDTO;
using APForums.Client.Data.Interfaces;
using APForums.Client.Data.Structures;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APForums.Client.Data
{
    public class EventService : IEventService
    {
        private readonly ILoginService _loginService;
        HttpClient _httpClient;

        public EventService(ILoginService loginService)
        {
            _loginService = loginService;
            _httpClient = new HttpClient();
        }

        public async Task<BasicHttpResponseWithData<UserEvents>> GetUserEvents(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Events/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<UserEvents>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Events/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<UserEvents>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<UserEvents>
                {
                    Data = events,
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return new BasicHttpResponseWithData<UserEvents>
                {
                    Status = HttpStatusCode.Forbidden,
                    Error = "Unable to fetch events for users, forbiddean access!"
                };
            }
            else
            {
                return new BasicHttpResponseWithData<UserEvents>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to fetch events!"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<EventWithInterest>> GetSingleEventWithInterest(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_EVENTS}/Interest/Event/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<EventWithInterest> {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_EVENTS}/Interest/Event/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var @event = JsonSerializer.Deserialize<EventWithInterest>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<EventWithInterest>
                {
                    Data = @event,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<EventWithInterest>
                {
                    Error = "Failed to fetch event information!",
                    Status = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<BasicUser>>> GetEventInterestedUsers(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_EVENTS}/Interest/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<IEnumerable<BasicUser>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_EVENTS}/Interest/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<IEnumerable<BasicUser>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<IEnumerable<BasicUser>>
                {
                    Data = users,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<IEnumerable<BasicUser>>
                {
                    Error = "Unable to fetch list of interested users!",
                    Status = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<BasicHttpResponse> AddEventInterest(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_EVENTS}/Interest/Add/{id}", new StringContent(""));
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
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_EVENTS}/Interest/Add/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return new BasicHttpResponse
                {
                    Error = "User is already interested in this event!",
                    Status = HttpStatusCode.InternalServerError,
                };
            } 
            else
            {
                return new BasicHttpResponse
                {
                    Error = "Unable to process this request, please try again!",
                    Status = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<BasicHttpResponse> RemoveEventInterest(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_EVENTS}/Interest/Remove/{id}");
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
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_EVENTS}/Interest/Remove/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return new BasicHttpResponse
                {
                    Error = "User is not interested in this event!",
                    Status = HttpStatusCode.InternalServerError,
                };
            }
            else
            {
                return new BasicHttpResponse
                {
                    Error = "Unable to process this request, please try again!",
                    Status = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<BasicHttpResponseWithData<int>> AddClubEvent(int clubId, Event @event)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(@event);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_EVENTS}/Club/{clubId}", content);
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
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_EVENTS}/Club/{clubId}", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var eventIDString = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<int>
                {
                    Data = int.Parse(eventIDString),
                    Status = HttpStatusCode.OK,
                };
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return new BasicHttpResponseWithData<int>
                {
                    Error = "User is not part of the commitee for this club!",
                    Status = HttpStatusCode.Forbidden,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<int>
                {
                    Error = "Unable to process this request, please try again!" + response.StatusCode.ToString(),
                    Status = HttpStatusCode.BadRequest,
                };
            }
        }

    }
}
