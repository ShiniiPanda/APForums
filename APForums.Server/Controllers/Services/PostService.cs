using APForums.Server.Controllers.Services.Interfaces;
using APForums.Server.Data;
using APForums.Server.Models;
using APForums.Server.Models.Types;
using Microsoft.EntityFrameworkCore;

namespace APForums.Server.Controllers.Services
{
    public class PostService : IPostService
    {

        private readonly ForumsDbContext _context;

        public PostService(ForumsDbContext context)
        {
            _context = context;
        }


        public void AddTagsToPost(ref Post post, IEnumerable<int> tagsIds)
        {
            if (tagsIds.Count() == 0) { return; }
            post.PostTags.Clear();
            var tags = _context.PostTags.Where(pt => tagsIds.Contains(pt.Id)).ToList();
            foreach (var tag in tags)
            {
                post.PostTags.Add(tag);
            }
        }


        public bool UserCanPost(Forum forum, User user)
        {
            var forumVisiblity = forum.Visibility;

            if (forumVisiblity == ForumVisibility.Public)
            {
                return true;   
            }
            else if (forumVisiblity == ForumVisibility.Intake)
            {
                if (user.IntakeCode == forum.Intake)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            else if (forum.Visibility == ForumVisibility.Club)
            {
                if (_context.UserClubs.Where(uc => uc.UserId == user.Id && uc.ClubId == forum.ClubId).Any())
                {
                    return true;
                } else
                {
                    return false;
                }                  
            } else
            {
                return true;
            }
        }

        public int GetTotalPosts()
        {
            return _context.Posts.Count();
        }
    }
}
