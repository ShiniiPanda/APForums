using APForums.Client.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Structures
{
    public class AuthResponse : HttpResponseHandle<AuthStatus>
    {
        
        public AuthResponse() { }

        public AuthResponse(LoginResponse response)
        {
            AccessToken = response.AccessToken;
            RefreshToken = response.RefreshToken;
        }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

    }

    public enum AuthStatus
    {
        Success,
        Unauthroized,
        Failed
    }
}
