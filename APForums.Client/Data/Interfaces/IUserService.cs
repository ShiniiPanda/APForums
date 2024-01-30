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
    public interface IUserService
    {
        Task<BasicHttpResponse> FollowUser(int id);
        Task<BasicHttpResponseWithData<IEnumerable<BasicUser>>> GetUserConnections(int id, string type = "Followers");
        Task<BasicHttpResponseWithData<HomePage>> GetUserHomePage();
        Task<User> GetUserInfoAsync(int userId);
        Task<BasicHttpResponseWithData<Profile>> GetUserProfile(int id);
        Task<BasicHttpResponse> UnfollowUser(int id);
        Task<bool> UpdateCachedUserInfo(int id);
        Task<BasicHttpResponse> UpdatePicture(int id, string picture);
    }
}
