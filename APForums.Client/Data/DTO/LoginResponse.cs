using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class LoginResponse
    {

        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

    }
}
