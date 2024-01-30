using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APForums.Client.Data.DTO;
using APForums.Client.Data.Structures;

namespace APForums.Client.Data.Interfaces
{
    public interface ILoginService
    {

        Task<AuthResponse> Auth(LoginRequest loginRequest);

        Task<AuthResponse> Refresh(LoginResponse loginResponse);
        Task SetAuthInfo(string accessToken, string refreshToken);
    }
}
