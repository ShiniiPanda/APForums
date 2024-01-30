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
    public class ClubService : IClubService
    {

        HttpClient _httpClient;

        private readonly ILoginService _loginService;

        public ClubService(ILoginService loginService)
        {
            _httpClient = new HttpClient();
            _loginService = loginService;
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<Club>>> GetAllClubs(string search = "")
        {
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_CLUBS}?search={search}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<IEnumerable<Club>> {
                    Data = JsonSerializer.Deserialize<IEnumerable<Club>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK
                };
            }
            else
            {
                return new BasicHttpResponseWithData<IEnumerable<Club>>
                {
                    Error = "Unable to fetch club information!",
                    Status = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<Club> GetClub(int clubId)
        {
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_CLUBS}/{clubId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Club>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                return null;
            }
        }

        public async Task<int> GetClubCount(int clubId)
        {
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_CLUBS}/{clubId}/Member/Count");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return int.Parse(result);
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetMemberStatus(int clubId, int userId)
        {
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_CLUBS}/{clubId}/Member/{userId}/Role");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return int.Parse(result);
            }
            else
            {
                return 0;
            }
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<ClubWithRole>>> GetUserClubs(int id)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<IEnumerable<ClubWithRole>>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_CLUBS}/User/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<IEnumerable<ClubWithRole>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_CLUBS}/User/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<IEnumerable<ClubWithRole>>
                {
                    Data = JsonSerializer.Deserialize<IEnumerable<ClubWithRole>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<IEnumerable<ClubWithRole>>
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<SingleClub>> GetSingleClubResponse(int id)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<SingleClub>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Club/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<SingleClub>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Club/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<SingleClub>
                {
                    Data = JsonSerializer.Deserialize<SingleClub>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new BasicHttpResponseWithData<SingleClub>
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Unable to fetch club information, club was not found!"
                    };
                }
                return new BasicHttpResponseWithData<SingleClub>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponse> JoinClub(int clubId)
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
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_CLUBS}/Join/{clubId}", new StringContent(""));
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_CLUBS}/Join/{clubId}", new StringContent(""));

            }
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
               return new BasicHttpResponse
               {
                   Status = HttpStatusCode.OK,
               };
            } else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Error = "User has already joined this club!"
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Failed to join club."
                };
            }                
        }

        public async Task<BasicHttpResponse> LeaveClub(int clubId)
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
            var response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_CLUBS}/Leave/{clubId}");
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
                response = await _httpClient.DeleteAsync($"{ServicesApiRoutes.API_CLUBS}/Leave/{clubId}");

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
                        Error = "User is not affiliated with this club."
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Failed to join club."
                };
            }
        }

        public async Task<BasicHttpResponseWithData<IEnumerable<BasicUserWithClubRole>>> GetClubMembers(int clubId)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<IEnumerable<BasicUserWithClubRole>>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_CLUBS}/{clubId}/Member");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<IEnumerable<BasicUserWithClubRole>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_CLUBS}/{clubId}/Member");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<IEnumerable<BasicUserWithClubRole>>
                {
                    Data = JsonSerializer.Deserialize<IEnumerable<BasicUserWithClubRole>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                return new BasicHttpResponseWithData<IEnumerable<BasicUserWithClubRole>>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponse> ChangeClubMemberStatus(int clubId, int userId, int role)
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
            var response = await _httpClient.PutAsync($"{ServicesApiRoutes.API_CLUBS}/{clubId}/Member/{userId}/Role/{role}", new StringContent(""));
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
                response = await _httpClient.PutAsync($"{ServicesApiRoutes.API_CLUBS}/{clubId}/Member/{userId}/Role/{role}", new StringContent(""));

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Forbidden,
                        Error = "Forbidden opperation, only club leader may change member roles!"
                    };
                } 
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Error = "User does not belong to the specified club!"
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }
    }
}
