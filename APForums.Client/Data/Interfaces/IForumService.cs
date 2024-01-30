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
    public interface IForumService
    {
        Task<BasicHttpResponseWithData<int>> AddClubForum(int clubId, Forum forum);
        Task<BasicHttpResponseWithData<Forum>> GetForum(int forumId);
        Task<BasicHttpResponseWithData<IEnumerable<ForumWithRecentPost>>> GetSubscribedForums(int id);
        Task<BasicHttpResponseWithData<PaginatedList<ForumWithRecentPost>>> GetUserForums(int userId, int page, int sort = 0, int size = 5, string search = "");
        Task<BasicHttpResponseWithData<bool>> GetUserSubscriptionStatus(int userId, int forumId);
        Task<BasicHttpResponse> Subscribe(int userId, int forumId);
        Task<BasicHttpResponse> Unsubscribe(int userId, int forumId);
    }
}
