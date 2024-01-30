using APForums.Client.Data.DTO;
using APForums.Client.Data.DTO.PagesDTO;
using APForums.Client.Data.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Interfaces
{
    public interface ITagService
    {
        Task<BasicHttpResponse> AddProfileTag(int id);
        Task<BasicHttpResponseWithData<IEnumerable<ProfileTag>>> GetAllProfileTags();
        Task<BasicHttpResponseWithData<IEnumerable<ProfileTag>>> GetProfileTags(int id);
        Task<BasicHttpResponseWithData<SocialProfileTag>> GetSocialProfileTag(int id);
        Task<BasicHttpResponse> RemoveProfileTag(int id);
    }
}
