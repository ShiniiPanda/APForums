using APForums.Server.Controllers.Services.Interfaces;
using APForums.Server.Data;
using APForums.Server.Data.DTO;
using APForums.Server.Data.DTO.PageDTOS;
using APForums.Server.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {

        private readonly ForumsDbContext _context;
        private readonly IIdentityService _identityService;

        public TagsController(ForumsDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        // GET: api/<TagsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileTagDTO>>> GetProfileTags()
        {
            if (_context.ProfileTags == null)
            {
                return NotFound();
            }

            var tags = await _context.ProfileTags.ToListAsync();
            var dtoList = new List<ProfileTagDTO>();

            foreach (var tag in tags)
            {
                dtoList.Add(new ProfileTagDTO(tag));
            }

            return dtoList;
        }

        [HttpPost]
        public async Task<IActionResult> PostProfileTag(ProfileTag tag)
        {
            if (_context.ProfileTags == null)
            {
                return NotFound();
            }

            _context.ProfileTags.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfileTag", new { Id = tag.Id }, tag);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileTagDTO>> GetProfileTag(int id)
        {
            if (_context.ProfileTags == null)
            {
                return NotFound();
            }

            var tag = await _context.ProfileTags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return new ProfileTagDTO(tag);
        }

        [HttpGet("Social/{id}")]
        [Authorize]
        public async Task<ActionResult<SocialTagResponse>> GetUserRecommendationsPerTag(int id)
        {

            Console.WriteLine("TAG ID IS: " + id);
            
            if (_context.ProfileTags == null)
            {
                return NotFound();
            }

            var tag = await _context.ProfileTags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var response = new SocialTagResponse();

            response.Tag = new ProfileTagDTO(tag);

            response.Subscribed = _context.UsersProfileTags.Where(r => r.UserId == userId && r.ProfileTagId == id).Any();

            var followIds = await _context.Connections.Where(c => c.FollowerId == userId).Select(u => u.FollowedId).ToListAsync();

            var usersWithTag = await _context.UsersProfileTags
                .Where(r => r.ProfileTagId == id)
                .Select(u => new BasicUserDTO(u.User))
                .AsNoTracking()
                .ToListAsync();

            foreach (var user in usersWithTag)
            {
                var dto = new UserConnectionDTO();
                if (user.Id == userId) continue;
                dto.User = user;
                if (followIds.Contains(user.Id))
                {
                    followIds.Remove(user.Id);
                    dto.Connection = 1;
                }
                response.Users.Add(dto);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost("User/Add/{id}")]
        public async Task<IActionResult> AddTag(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            var tag = await _context.ProfileTags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }


            if (_context.UsersProfileTags.Where(u => u.UserId == userId && u.ProfileTagId == id).Any())
            {
                return Problem("User profile already contains this tag!");
            }

            _context.UsersProfileTags.Add(new UserProfileTags
            {
                UserId = userId,
                ProfileTagId = id
            });

            await _context.SaveChangesAsync();

            return Ok("Success!");
        }

        [Authorize]
        [HttpDelete("User/Remove/{id}")]
        public async Task<IActionResult> RemoveTag(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            var tag = await _context.ProfileTags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            var userTag = await _context.UsersProfileTags.Where(u => u.UserId == userId && u.ProfileTagId == id).FirstOrDefaultAsync();

            if (userTag == null)
            {
                return Problem("User profile does not have this tag!");
            }

            user.UserProfileTags.Remove(userTag);

            await _context.SaveChangesAsync();

            return Ok("Success!");
        }

        [HttpGet("User/{id}")]
        public async Task<ActionResult<IEnumerable<ProfileTagDTO>>> GetTags(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var userTags = await _context.Users.Where(u => u.Id == id).SelectMany(u => u.ProfileTags).Select(t => new ProfileTagDTO(t))
                .AsNoTracking()
                .ToListAsync();

            if (userTags == null) { return NotFound(); }

            return userTags;
        }
    }
}
