using APForums.Server.Controllers.Services;
using APForums.Server.Controllers.Services.Interfaces;
using APForums.Server.Data;
using APForums.Server.Data.DTO;
using APForums.Server.Data.DTO.PageDTOS;
using APForums.Server.Models.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Cryptography.Xml;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {

        private readonly ForumsDbContext _context;

        private readonly IIdentityService _identityService;

        public PagesController(ForumsDbContext forumsDbContext, IIdentityService identityService)
        {
            _context = forumsDbContext;
            _identityService = identityService;
        }

        [HttpGet("Profile/{id}")]
        [Authorize]
        public async Task<ActionResult<ProfileResponse>> GetProfile(int id)
        {

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var response = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new ProfileResponse
                {
                    User = new UserDTO(u),
                    Clubs = u.Clubs.Select(c => new ClubDTO(c)).ToList(),
                    ProfileTags = u.ProfileTags.Select(pt => new ProfileTagDTO(pt)).ToList(),
                    Socials = u.Socials.Select(s => new SocialDTO(s)).ToList(),
                }).AsSplitQuery().AsNoTracking().FirstOrDefaultAsync();

            if (response == null)
            {
                return NotFound();
            }

            var UserConnections = await _context.Connections.Where(c => c.FollowerId == id || c.FollowedId == id)
                .Select(c => new
                {
                    c.FollowedId,
                    c.FollowerId
                }).ToListAsync();

            response.NumberOfFollowers = UserConnections.Where(uc => uc.FollowedId == id).Count();
            response.NumberOfFollowing = UserConnections.Where(uc => uc.FollowerId == id).Count();

            if (userId == id)
            {
                return response;
            }

            response.IsFollower = UserConnections.Where(c=> c.FollowedId == userId).Any();
            response.IsFollowed = UserConnections.Where(c => c.FollowerId == userId).Any();

            return response;
        }

        [HttpGet("Events/{id}")]
        [Authorize]
        public async Task<ActionResult<EventsResponse>> GetEvents(int id)
        {

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            if (id != userId)
            {
                return Forbid();
            }

            var response = new EventsResponse();

            response.PrivateEvents = await _context.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.Clubs)
                .SelectMany(c => c.Events.Where(e => e.Visibility == EventVisibility.Private && e.StartDate > DateTime.UtcNow.AddDays(-2)), 
                (c, e) => new { ClubName = c.Name, ClubEvent = e })
                .OrderBy(e => e.ClubEvent.StartDate)
                .GroupJoin(
                 _context.EventInterests.Where(ei => ei.UserId == userId),
                 e => e.ClubEvent.Id,
                 i => i.EventId,
                 (e, i) => new EventWIthInterestDTO
                 {
                     Event = new EventDTO(e.ClubEvent)
                     {
                         ClubName = e.ClubName,
                     },
                     IsInterested = i.Any()
                 }
                )
                .AsNoTracking()
                .ToListAsync();

            response.PublicEvents = await _context.Events
                .Where(e => e.Visibility == EventVisibility.Public && e.StartDate  > DateTime.UtcNow.AddDays(-2))
                .Select(e => new { ClubName = e.Club == null ? "APU" : e.Club.Name, Event = e })
                .OrderBy(e => e.Event.StartDate)
                .GroupJoin(
                 _context.EventInterests.Where(ei => ei.UserId == userId),
                 e => e.Event.Id,
                 i => i.EventId,
                 (e, i) => new EventWIthInterestDTO
                 {
                     Event = new EventDTO(e.Event)
                     {
                         ClubName = e.ClubName,
                     },
                     IsInterested = i.Any()
                 }
                )
                .AsNoTracking()
                .ToListAsync();

            return response;
        }

        [HttpGet("Post/{id}")]
        [Authorize]
        public async Task<ActionResult<SinglePostResponse>> GetSinglePost(int id)
        {
            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var post = await _context.Posts.Where(P => P.Id == id)
                .Select(p => new SinglePostResponse
                {
                    Post = new PostDTO(p) { User = new BasicUserDTO(p.User) },
                    Tags = p.PostTags.Select(t => new PostTagDTO(t)).ToList(),
                    Impressions = p.Impressions.Select(i => new PostImpressionDTO(i)).ToList(),
                    ForumName = p.Forum.Name
                })
                .AsSplitQuery()
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }

            post.Comments = await _context.Comments.Include(c => c.Impressions)
                .Where(c => c.PostId == id)
                .OrderBy(c => c.PostedDate)
                .Select(c => new CommentDTO(c)
                {
                    User = new BasicUserDTO(c.User)
                })
                .AsNoTracking()
                .ToListAsync();

            return post;
        }

        [HttpGet("Club/{id}")]
        [Authorize]
        public async Task<ActionResult<SingleClubResponse>> GetSingleClub(int id)
        {
            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var userRelation = await _context.UserClubs.Where(uc => uc.ClubId == id && uc.UserId == userId).FirstOrDefaultAsync();

            var response = new SingleClubResponse();

            if (userRelation != null)
            {
                response = await _context.Clubs.Where(c => c.Id == id)
                    .Select(c => new SingleClubResponse
                    {
                        Club = new ClubDTO(c),
                        Forums = c.Forums.Select(f => new ForumDTO(f)).ToList(),
                        Events = c.Events.Where(e => e.StartDate > DateTime.UtcNow.AddDays(-2))
                            .OrderBy(e => e.StartDate)
                            .GroupJoin(_context.EventInterests.Where(ei => ei.UserId == userId),
                             e => e.Id,
                             i => i.EventId,
                             (e, i) => new EventWIthInterestDTO
                             {
                                 Event = new EventDTO(e),
                                 IsInterested = i.Any()
                             })
                            .ToList()
                    })
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync();
            } else
            {
                response = await _context.Clubs.Where(c => c.Id == id)
               .Select(c => new SingleClubResponse
                   {
                       Club = new ClubDTO(c),
                       Forums = c.Forums.Where(f => f.Visibility == ForumVisibility.Public)
                            .Select(f => new ForumDTO(f)).ToList(),
                       Events = c.Events
                           .Where(e => e.StartDate > DateTime.UtcNow.AddDays(-2) && e.Visibility == EventVisibility.Public)
                           .OrderBy(e => e.StartDate)
                           .GroupJoin(_context.EventInterests.Where(ei => ei.UserId == userId),
                            e => e.Id,
                            i => i.EventId,
                            (e, i) => new EventWIthInterestDTO
                            {
                                Event = new EventDTO(e),
                                IsInterested = i.Any()
                            })
                           .ToList()
                   })
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync();
            }

            if (response == null)
            {
                return NotFound();
            }

            response.MemberCount = _context.UserClubs.Where(uc => uc.ClubId == id).Count();
            response.Role = userRelation == null ? 0 : (int)userRelation.Role;

            return response;
        }

        [HttpGet("Home")]
        [Authorize]
        public async Task<ActionResult<HomePageResponse>> GetHomePage()
        {
            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var response = new HomePageResponse();

            response.Activities = await _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .Include(ua => ua.Activity)
                .OrderBy(ua => ua.Read).ThenByDescending(ua => ua.Activity.Date)
                .Take(5)
                .Select(ua => new ActivityDTO(ua.Activity) { Read = ua.Read })
                .AsNoTracking()
                .ToListAsync();

            response.Posts = await _context.Posts
                .Include(p => p.Forum)
                .Where(p => p.Forum.Visibility == ForumVisibility.Public)
                .OrderByDescending(p => p.PublishedDate)
                .Take(5)
                .Select(p => new PostDTO(p) { User = new BasicUserDTO(p.User), ForumName = p.Forum.Name })
                .AsNoTracking()
                .ToListAsync();

            return response;
        }

    }

}
