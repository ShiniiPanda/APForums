using APForums.Client.Data.DTO;
using APForums.Client.Data.Interfaces;
using APForums.Client.Data.Storage;
using APForums.Client.Data.Structures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APForums.Client.Data
{
    public class LoginService : ILoginService
    {
        HttpClient _httpClient;

        public LoginService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<AuthResponse> Auth(LoginRequest loginRequest)
        {
            var content = JsonSerializer.Serialize(loginRequest);
            var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_USERS}/Authenticate", jsonContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<LoginResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new AuthResponse(obj)
                {
                    Status = AuthStatus.Success
                };
            } else
            {
                return new AuthResponse
                {
                    Status = AuthStatus.Failed
                };
            }
        }

        public async Task<AuthResponse> Refresh(LoginResponse loginResponse)
        {
            var content = JsonSerializer.Serialize(loginResponse);
            var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_USERS}/Refresh", jsonContent);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<LoginResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new AuthResponse(obj)
                {
                    Status = AuthStatus.Success
                };
            }
            else
            {
                return new AuthResponse
                {
                    Status = AuthStatus.Unauthroized
                };
            }
        }

        public async Task SetAuthInfo(string accessToken, string refreshToken)
        {
            var newAuthInfo = new AuthInfo();
            newAuthInfo.ReadAuthResponse(accessToken, refreshToken);
            var newAuthInfoJson = JsonSerializer.Serialize(newAuthInfo);
            SecureStorage.Default.Remove(nameof(Settings.authInfo));
            await SecureStorage.Default.SetAsync(nameof(Settings.authInfo), newAuthInfoJson);
            Settings.authInfo = newAuthInfo;
        }

        /*public async Task<string> GetUserInfo(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                return null;
            }
        }*/

    }
}
