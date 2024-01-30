using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APForums.Server.Data;
using APForums.Server.Models;
using APForums.Server.Data.DTO;
using System.Security.Claims;
using APForums.Server.Models.Types;
using Microsoft.AspNetCore.Authorization;
using APForums.Server.Controllers.Services.Interfaces;
using System.Security.Cryptography.Xml;
using System.Drawing;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumsController : ControllerBase
    {
        private readonly ForumsDbContext _context;

        private readonly IIdentityService _identityService;

        private readonly IClubService _clubService;

        public ForumsController(ForumsDbContext context, IIdentityService identityService, IClubService clubService)
        {
            _context = context;
            _identityService = identityService;
            _clubService = clubService;
        }

        // GET: api/Forums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ForumDTO>>> GetForums()
        {
          if (_context.Forums == null)
          {
             return NotFound();
          }

            return await _context.Forums.Select(f => new ForumDTO(f)).ToListAsync();

        }

        [HttpGet("User/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ForumDTO>>> GetUserAccessibleForums(int userId, int type = 0, int page = 0, int size = 5, string search = "")
        {
            if (_context.Forums == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int requestingUserId))
            {
                return Unauthorized();
            }

            if (requestingUserId != userId)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var baseQuery = _context.Forums.AsQueryable();

            switch (type)
            {
                case 1:
                    // Fetching forums for the user's intake code
                    if (user.IntakeCode == null) goto default;
                    baseQuery = baseQuery.Where(f => f.Intake == user.IntakeCode);
                    break;
                case 2:
                    // Fetching forums that fall under clubs only
                    List<int> clubIds = await _context.UserClubs.Where(uc => uc.UserId == userId).Select(uc => uc.ClubId).ToListAsync();
                    baseQuery = baseQuery.Where(f => f.ClubId != null && clubIds.Contains((int)f.ClubId!));
                    break;
                default:
                    // Fetching all forums with public visibility, regardless of whether they fall under clubs or not
                    baseQuery = baseQuery.Where(f => f.Visibility == ForumVisibility.Public);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                baseQuery = baseQuery.Where(f => f.Name.Contains(search));
            }

            var totalForumCount = baseQuery.Count();

            if (page == 0)
            {
                var allforums = await baseQuery
                .Select(f => new ForumWithRecentPostDTO {
                    Forum = new ForumDTO(f) { ClubName = f.ClubId == null ? null : f.Club!.Name },
                    Post = f.Posts.OrderByDescending(p => p.PublishedDate)
                    .Select(p => new PostDTO(p) { User = new BasicUserDTO(p.User) })
                    .FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();

                return Ok(allforums);
            }

            var forums = await baseQuery
                .Skip((page - 1) * size)
                .Take(size)
                .Select(f => new ForumWithRecentPostDTO
                {
                    Forum = new ForumDTO(f) { ClubName = f.ClubId == null ? null : f.Club!.Name },
                    Post = f.Posts.OrderByDescending(p => p.PublishedDate)
                    .Select(p => new PostDTO(p) { User = new BasicUserDTO(p.User) })
                    .FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();

            var totalPageCount = (int)Math.Ceiling((double)totalForumCount / size);
            var response = new PaginatedListDTO<ForumWithRecentPostDTO>
            {
                PageSize = size,
                CurrentPage = page,
                TotalItems = totalForumCount,
                TotalPages = totalPageCount,
                Items = forums
            };

            return Ok(response);
        }

        [HttpGet("User/{userId}/Subscriptions")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ForumWithRecentPostDTO>>> GetUserSubscribedForums(int userId)
        {
            if (_context.Forums == null || _context.Users == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int requestingUserId))
            {
                return Unauthorized();
            }

            var forums = await _context.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.SubscribedForums)
                .OrderBy(f => f.Name)
                .Select(f => new ForumWithRecentPostDTO
                {
                    Forum = new ForumDTO(f) { ClubName = f.ClubId == null ? null : f.Club!.Name },
                    Post = f.Posts.OrderByDescending(p => p.PublishedDate)
                    .Select(p => new PostDTO(p) { User = new BasicUserDTO(p.User) })
                    .FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();


            return forums;
        }

        [HttpGet("User/{userId}/Subscriptions/{forumId}")]
        [Authorize]
        public async Task<ActionResult<bool>> GetUserSubscriptionStatus(int userId, int forumId)
        {
            if (_context.Forums == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int requestingUserId))
            {
                return Unauthorized();
            }

            var subscription = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.SubscribedForums
                .Where(f => f.Id == forumId).Any())
                .FirstOrDefaultAsync();

            return subscription;
        }


        [HttpPost("User/{userId}/Subscriptions/{forumId}")]
        [Authorize]
        public async Task<ActionResult<Forum>> Subscribe(int userId, int forumId)
        {
            if (_context.Forums == null || _context.Users == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int requestingUserId))
            {
                return Unauthorized();
            }

            if (requestingUserId != userId)
            {
                return BadRequest();
            }

            var user = await _context.Users.Where(u => u.Id == userId).Include(u => u.SubscribedForums).FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            if (user.SubscribedForums.Where(sf => sf.Id == forumId).Any())
            {
                return Problem("User already subscribed for this forum.");
            }

            var forum = await _context.Forums.FindAsync(forumId);

            if (forum == null)
            {
                return NotFound();
            }

            user.SubscribedForums.Add(forum);

            var activity = new Activity
            {
                Title = "Forum Subscriptions",
                Message = $"You have successfully subscribed to {forum.Name} forum!",
                Source = "Forums",
                Date = DateTime.Now
            };

            user.Activities.Add(activity);

            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        [HttpDelete("User/{userId}/Subscriptions/{forumId}")]
        [Authorize]
        public async Task<ActionResult<Forum>> Unsubscribe(int userId, int forumId)
        {
            if (_context.Forums == null || _context.Users == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int requestingUserId))
            {
                return Unauthorized();
            }

            if (requestingUserId != userId)
            {
                return BadRequest();
            }

            var user = await _context.Users.Where(u => u.Id == userId).Include(u => u.SubscribedForums).FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            var forum = user.SubscribedForums.Where(sf => sf.Id == forumId).FirstOrDefault();

            if (forum == null)
            {
                return Problem("User is not subscribed to this forum.");
            }

            user.SubscribedForums.Remove(forum);

            var activity = new Activity
            {
                Title = "Forum Subscriptions",
                Message = $"You have successfully unsubscribed to {forum.Name} forum!",
                Source = "Forums",
                Date = DateTime.Now
            };

            user.Activities.Add(activity);

            await _context.SaveChangesAsync();

            return Ok("Success");
        }


        [HttpGet("Club/{id}")]
        public async Task<ActionResult<IEnumerable<ForumDTO>>> GetClubForums(int id)
        {
            if (_context.Forums == null || _context.Clubs == null)
            {
                return NotFound();
            }
            List<ForumDTO> result = new List<ForumDTO>();
            var club = await _context.Clubs.Include(c => c.Forums).FirstOrDefaultAsync(c => c.Id == id);
            if (club == null)
            {
                return NotFound();
            }

            foreach (var forum in club.Forums)
            {
                result.Add(new ForumDTO(forum));
            }
            return result;
        }

        [HttpPost("Club/{id}")]
        [Authorize]
        public async Task<ActionResult<Forum>> PostClubForum(ForumDTO forum, int id)
        {
            if (_context.Forums == null)
            {
                return NotFound();
            }

            if (forum == null) 
            {
                return BadRequest();
            }

            if (!_context.Clubs.Where(c => c.Id == id).Any()) 
            {
                return NotFound();
            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userIdInt = int.Parse(userId);

            var relation = await _context.UserClubs.Where(uc => uc.UserId == userIdInt && uc.ClubId == id).FirstOrDefaultAsync();

            if (relation  == null)
            {
                return NotFound();
            }

            if (!(relation.Role == ClubRole.Commitee || relation.Role == ClubRole.Leader))
            {
                return Forbid();
            }


            var model = new Forum(forum);

            var tracker = _context.Forums.Add(model);

            await _context.SaveChangesAsync();

            return Ok(tracker.Entity.Id);
        }

        [HttpPut("Club/{clubId}/Update/{forumId}")]
        [Authorize]
        public async Task<ActionResult<Forum>> UpdateClubForum(ForumDTO forum, int clubId, int forumId)
        {
            if (_context.Forums == null)
            {
                return NotFound();
            }

            if (forum == null || forumId != forum.Id)
            {
                return BadRequest();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var IsCommitee = await _clubService.IsMemberCommittee(userId, clubId);

            if (!IsCommitee)
            {
                return Forbid();
            }

            var model = new Forum {
                Id = (int)forum.Id,
                Name = forum.Name!,
                Description = forum.Description,
                Intake = forum.Intake,
                Visibility = (ForumVisibility)forum.Visibility!,
                ClubId = clubId
            };

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(forumId))
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

        [HttpDelete("Club/{clubId}/Delete/{forumId}")]
        [Authorize]
        public async Task<ActionResult<Forum>> DeleteClubForum(int clubId, int forumId)
        {
            if (_context.Forums == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var IsCommitee = await _clubService.IsMemberCommittee(userId, clubId);

            if (!IsCommitee)
            {
                return Forbid();
            }

            var forum = await _context.Forums.FindAsync(forumId);

            if (forum == null)
            {
                return NotFound();
            }

            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Forums/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ForumDTO>> GetForum(int id)
        {
            if (_context.Forums == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var forum = await _context.Forums.Where(f => f.Id == id)
                .Select(f => new ForumDTO(f)
                {
                    ClubName = f.ClubId == null ? null : f.Club!.Name
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (forum == null)
            {
                return NotFound();
            }

            return forum;
        }

        // POST: api/Forums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Forum>> PostForum(Forum forum)
        {
            if (_context.Forums == null)
            {
                return Problem("Entity set 'ForumsDbContext.Forums'  is null.");
            }
            _context.Forums.Add(forum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetForum", new { id = forum.Id }, forum);
        }

        // PUT: api/Forums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForum(int id, Forum forum)
        {
            if (id != forum.Id)
            {
                return BadRequest();
            }

            _context.Entry(forum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(id))
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

        // DELETE: api/Forums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForum(int id)
        {
            if (_context.Forums == null)
            {
                return NotFound();
            }
            var forum = await _context.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound();
            }

            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForumExists(int id)
        {
            return (_context.Forums?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
