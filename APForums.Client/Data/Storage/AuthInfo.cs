using APForums.Client.Data.DTO;
using APForums.Client.Data.Structures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Storage
{
    public class AuthInfo
    {

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int Id { get; set; }

        public string TPNumber { get; set; }

        public void ReadAuthResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(AccessToken) as JwtSecurityToken;
            Id = int.Parse(token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.NameId).Value);
            TPNumber = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.UniqueName).Value;
        }


    }
}
