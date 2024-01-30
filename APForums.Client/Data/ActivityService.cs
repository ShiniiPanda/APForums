using APForums.Client.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APForums.Client.Data.Structures;
using APForums.Client.Data.DTO;

namespace APForums.Client.Data
{
    public class ActivityService : IActivityService
    {
        HttpClient _httpClient;
        private readonly ILoginService _loginService;

        public ActivityService(ILoginService loginService)
        {
            _httpClient = new HttpClient();
            _loginService = loginService;
        }

        public async Task<BasicHttpResponseWithData<PaginatedList<Activity>>> GetUserActivities(int page = 1, int size = 10, int type = 0)
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
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_ACTIVITIES}?page={page}&size={size}&type={type}");
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
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_ACTIVITIES}?page={page}&size={size}&type={type}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new()
                {
                    Data = JsonSerializer.Deserialize<PaginatedList<Activity>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to fetch user activities!"
                };
            }
        }

        public async Task<BasicHttpResponse> MarkActivityAsRead(int id)
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
            var response = await _httpClient.PutAsync($"{ServicesApiRoutes.API_ACTIVITIES}/Read/{id}", new StringContent(""));
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
                response = await _httpClient.PutAsync($"{ServicesApiRoutes.API_ACTIVITIES}/Read/{id}", new StringContent(""));

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new()
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to fetch user activities!"
                };
            }
        }


    }
}
