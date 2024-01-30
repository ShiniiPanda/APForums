using APForums.Client.Data.DTO;
using APForums.Client.Data.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Interfaces
{
    public interface ISocialService
    { 
        Task<BasicHttpResponseWithData<SocialLink>> AddSocialLink(SocialLink social);
        Task<BasicHttpResponse> DeleteSocialLink(int id);
        Task<BasicHttpResponseWithData<IEnumerable<SocialLink>>> GetUserSocials(int id);
        Task<BasicHttpResponse> UpdateSocialLink(SocialLink social);
    }
}
