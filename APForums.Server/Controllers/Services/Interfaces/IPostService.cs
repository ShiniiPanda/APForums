using APForums.Server.Models;

namespace APForums.Server.Controllers.Services.Interfaces
{
    public interface IPostService
    {
        void AddTagsToPost(ref Post post, IEnumerable<int> tagsIds);
        int GetTotalPosts();
        bool UserCanPost(Forum forum, User user);
    }
}
