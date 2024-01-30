using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APForums.Server.Data;
using APForums.Server.Models;
using Microsoft.AspNetCore.Authorization;
using APForums.Server.Controllers.Services;
using APForums.Server.Models.Types;
using APForums.Server.Data.DTO;
using APForums.Server.Controllers.Services.Interfaces;
using System.Drawing.Printing;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ForumsDbContext _context;

        private readonly IIdentityService _identityService;

        private readonly IClubService _clubService;

        private readonly IPostService _postService;

        public PostsController(ForumsDbContext context, IIdentityService identityService, IClubService clubService, IPostService postService)
        {
            _context = context;
            _identityService = identityService;
            _clubService = clubService;
            _postService = postService;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
          if (_context.Posts == null)
          {
              return NotFound();
          }
            return await _context.Posts.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
          if (_context.Posts == null)
          {
              return NotFound();
          }
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPost(int id, Post post)
        { 
            if (id != post.Id)
            {
                return BadRequest();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId,out string role))
            {
                return Unauthorized();
            }

            var authorized = false;

            if (role.Equals(IdentityService.AdminRoleClaimName) || post.UserId == userId)
            {
                authorized = true;
            }

            var forum = await _context.Forums.FindAsync(post.ForumId);

            if (forum != null && forum.ClubId != null)
            {
                var ClubAuthorization = await _clubService.IsMemberRole(userId, (int)forum.ClubId, ClubRole.Leader);
                if (ClubAuthorization)
                {
                    authorized = true;
                }
            }

            if (!authorized)
            {
                return Forbid();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> PostPost(PostDTO postDTO)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId, out string role))
            {
                return Unauthorized();
            }

            if (postDTO.User!.Id != userId)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return Unauthorized();
            }

            var forum = await _context.Forums.Where(f => f.Id == postDTO.ForumId).Include(f => f.SubscribedUsers).FirstOrDefaultAsync();

            if (forum == null)
            {
                return BadRequest();
            }

            if (!_postService.UserCanPost(forum, user))
            {
                return Forbid();
            }

            var postModel = new Post(postDTO);
            _postService.AddTagsToPost(ref postModel, postDTO.PostTags);

            var tracker = _context.Posts.Add(postModel);

            var activity = new Activity {

                Title = "Created a Post!",
                Source = "Posts",
                Message = $"You have successfully created a new post under {forum.Name}",
                Date = DateTime.Now,
            };

            user.Activities.Add(activity);

            if (forum.SubscribedUsers.Count() > 0)
            {
                var subscriptionActivity = new Activity
                {
                    Title = "New Forum Activity",
                    Source = "Subscriptions",
                    Message = $"({postDTO.Title}) was posted in {forum.Name}",
                    Date = DateTime.Now
                };

                var activityTracker = _context.Activities.Add(subscriptionActivity);

                await _context.SaveChangesAsync();

                foreach (var subscriber in forum.SubscribedUsers)
                {
                    if (subscriber.Id == userId) continue;
                    subscriber.UserActivities.Add(new UserActivity
                    {
                        ActivityId = activityTracker.Entity.Id,
                        UserId = subscriber.Id,
                    });
                }
            }

            await _context.SaveChangesAsync();

            return Ok(tracker.Entity.Id);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId, out string role))
            {
                return Unauthorized();
            }

            var authorized = false;

            if (role.Equals(IdentityService.AdminRoleClaimName) || post.UserId == userId)
            {
                authorized = true;
            }

            var forum = await _context.Forums.FindAsync(post.ForumId);

            if (forum != null && forum.ClubId != null)
            {
                var ClubAuthorization = await _clubService.IsMemberRole(userId, (int)forum.ClubId, ClubRole.Leader);
                if (ClubAuthorization)
                {
                    authorized = true;
                }
            }

            if (!authorized)
            {
                return Forbid();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("Forum/{id}")]
        [Authorize]
        public async Task<IActionResult> GetForumPosts(int id, int sort = 0, int page = 0, int size = 5, string search = "")
        {
            if (_context.Forums == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userID))
            {
                return Unauthorized();
            }

            var baseQuery = _context.Posts.Where(p => p.ForumId == id);

            switch (sort)
            {
                case 1:
                    // Sorting by total number of post impressions, in a descending order ;)
                    baseQuery = baseQuery.OrderByDescending(p => p.Impressions.Count());
                    break;
                case 2:
                    // Sorting by earliest date published
                    baseQuery = baseQuery.OrderBy(p => p.PublishedDate);
                    break;
                default:
                    // Sorting by latest date published
                    baseQuery = baseQuery.OrderByDescending(p => p.PublishedDate);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                baseQuery = baseQuery.Where(f => f.Title.ToLower().Contains(search));
            }

            if (page == 0)
            {
                var allposts = await baseQuery
                .Select(p => new PostDTO(p) { User = new BasicUserDTO(p.User) })
                .AsNoTracking()
                .ToListAsync();

                return Ok(allposts);
            }

            var totalPostCount = baseQuery.Count();

            var posts = await baseQuery
                .Skip((page - 1) * size)
                .Take(size)
                .Select(p => new PostDTO(p) { User = new BasicUserDTO(p.User), PostTags = p.PostTags.Select(t => t.Id).ToList() })
                .AsNoTracking()
                .ToListAsync();

            var totalPageCount = (int)Math.Ceiling((double)totalPostCount / size);
            var response = new PaginatedListDTO<PostDTO>
            {
                PageSize = size,
                CurrentPage = page,
                TotalItems = totalPostCount,
                TotalPages = totalPageCount,
                Items = posts
            };

            return Ok(response);
        }

        [HttpPost("Impression/Post/Add")]
        [Authorize]
        public async Task<ActionResult<Forum>> AddPostImpression(PostImpressionDTO impression)
        {
            if (_context.PostImpressions == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var existingImpression = await _context.PostImpressions
                .Where(p => p.UserId == impression.UserId && p.PostId == impression.PostId)
                .FirstOrDefaultAsync();

            if (existingImpression == null)
            {
                _context.PostImpressions.Add(new PostImpression(impression));
            }
            else if (existingImpression.Value != (ImpressionValue)impression.Value)
            {
                Console.WriteLine("The current value is " + impression.Value);
                existingImpression.Value = (ImpressionValue)impression.Value;
                existingImpression.LastUpdated = DateTime.UtcNow;
                _context.Entry(existingImpression).State = EntityState.Modified;
            }
            else
            {
                return Conflict();
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Impression/Post/Remove")]
        [Authorize]
        public async Task<ActionResult<Forum>> DeletePostImpression(PostImpressionDTO impression)
        {
            if (_context.PostImpressions == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var existingImpression = await _context.PostImpressions
                .Where(p => p.UserId == impression.UserId && p.PostId == impression.PostId)
                .FirstOrDefaultAsync();

            if (existingImpression == null)
            {
                return Problem("Impression does not exist, perhaps it was already removed!");
            }

            _context.PostImpressions.Remove(existingImpression);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Comment/Add")]
        [Authorize]
        public async Task<ActionResult<Forum>> AddPostComment(CommentDTO comment)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var post = await _context.Posts
                .Include(p => p.Forum)
                .Where(p => p.Id == comment.PostId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return BadRequest();
            }

            var user = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest();
            }

            if (!_postService.UserCanPost(post.Forum, user))
            {
                return Forbid();
            }

            var tracker = _context.Comments.Add(new Comment(comment));

            var activity = new Activity
            {
                Date = DateTime.UtcNow,
                Title = "Commented!",
                Message = $"You have commented on {post.Title}!",
                Source = "Posts"
            };

            user.Activities.Add(activity);

            await _context.SaveChangesAsync();

            return Ok(new CommentDTO(tracker.Entity) { User = new BasicUserDTO(tracker.Entity.User)});
        }

        [HttpPost("Impression/Comment/Add")]
        [Authorize]
        public async Task<ActionResult<Forum>> AddCommentImpression(CommentImpressionDTO impression)
        {
            if (_context.CommentImpressions == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var existingImpression = await _context.CommentImpressions
                .Where(c => c.UserId == impression.UserId &&  c.CommentId == impression.CommentId)
                .FirstOrDefaultAsync();

            if (existingImpression == null)
            {
                _context.CommentImpressions.Add(new CommentImpression(impression));
            }
            else if (existingImpression.Value != (ImpressionValue)impression.Value)
            {
                existingImpression.Value = (ImpressionValue)impression.Value;
                existingImpression.LastUpdated = DateTime.UtcNow;
                _context.Entry(existingImpression).State = EntityState.Modified;
            }
            else
            {
                return Conflict();
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Impression/Comment/Remove")]
        [Authorize]
        public async Task<ActionResult<Forum>> DeleteCommentImpression(CommentImpressionDTO impression)
        {
            if (_context.CommentImpressions == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var existingImpression = await _context.CommentImpressions
                .Where(c => c.UserId == impression.UserId && c.CommentId == impression.CommentId)
                .FirstOrDefaultAsync();

            if (existingImpression == null)
            {
                return Problem("Impression does not exist, perhaps it was already removed!");
            }

            _context.CommentImpressions.Remove(existingImpression);
            await _context.SaveChangesAsync();

            return Ok();
        }


        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
