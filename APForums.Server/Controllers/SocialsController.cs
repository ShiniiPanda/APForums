using APForums.Server.Controllers.Services.Interfaces;
using APForums.Server.Data.DTO;
using APForums.Server.Data;
using APForums.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APForums.Server.Models.Types;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialsController : ControllerBase
    {

        private readonly ForumsDbContext _context;
        private readonly IIdentityService _identityService;

        public SocialsController(ForumsDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        // GET: api/<TagsController>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SocialDTO>>> GetUserSocials(int id)
        {
            if (_context.Socials == null)
            {
                return NotFound();
            }

            if(!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var userSocials = await 
                _context.Users.Where(u => u.Id == id)
                .Select(u => u.Socials.Select(s => new SocialDTO(s)).ToList())
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (userSocials == null)
            {
                return NotFound();
            }

            return userSocials;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostUserSocial(SocialDTO social)
        {
            if (_context.ProfileTags == null)
            {
                return NotFound();
            }

            if (social == null)
            {
                return BadRequest();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            if (social.UserId != userId)
            {
                return BadRequest();
            }

            var socialModel = new Social(social);

            var tracker = _context.Socials.Add(socialModel);
            await _context.SaveChangesAsync();

            return Ok(tracker.Entity);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserSocial(SocialDTO social)
        {
            if (_context.ProfileTags == null)
            {
                return NotFound();
            }

            if (social == null)
            {
                return BadRequest();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            if (social.UserId != userId)
            {
                return BadRequest();
            }

            var userSocial = await _context.Socials.FindAsync(social.Id);

            if (userSocial == null)
            {
                return NotFound();
            }

            userSocial.Value = social.Value;
            userSocial.Type = (SocialLink)social.Type;
            userSocial.LastUpdated = DateTime.UtcNow;

            _context.Entry(userSocial).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserSocial(int id)
        {
            if (_context.ProfileTags == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var userSocial = await _context.Socials.FindAsync(id);

            if (userSocial == null)
            {
                return NotFound();
            }

            if (userSocial.UserId != userId)
            {
                return BadRequest();
            }

            _context.Socials.Remove(userSocial);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
