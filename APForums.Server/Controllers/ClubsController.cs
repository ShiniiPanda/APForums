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
using NuGet.Protocol;
using Microsoft.AspNetCore.Authorization;
using APForums.Server.Models.Types;
using System.Security.Claims;
using Microsoft.AspNetCore.Connections.Features;
using APForums.Server.Controllers.Services.Interfaces;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly ForumsDbContext _context;

        private readonly IIdentityService _identityService;

        private readonly IClubService _clubService;

        public ClubsController(ForumsDbContext context, IIdentityService identityService, IClubService clubService)
        {
            _context = context;
            _identityService = identityService;
            _clubService = clubService;
        }

        // GET: api/Clubs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClubDTO>>> GetClubs(string search = "")
        {
            if (_context.Clubs == null)
            {
                return NotFound();
            }

            var baseQuery = _context.Clubs.OrderBy(c => c.Name)
                .ThenByDescending(c => c.Type).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search)) {

                baseQuery = baseQuery.Where(c => c.Name.Contains(search) || c.Abbreviation.Contains(search));
            }

            return await baseQuery
                .Select(c => new ClubDTO(c))
                .AsNoTracking()
                .ToListAsync();
        }

        // GET: api/Clubs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClubDTO>> GetClub(int id)
        {
          if (_context.Clubs == null)
          {
              return NotFound();
          }
            var club = await _context.Clubs.FindAsync(id);

            if (club == null)
            {
                return NotFound();
            }

            return new ClubDTO(club);
        }

        // PUT: api/Clubs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClub(int id, Club club)
        {
            if (id != club.Id)
            {
                return BadRequest();
            }

            _context.Entry(club).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(id))
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

        // POST: api/Clubs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Club>> PostClub(Club club)
        {
          if (_context.Clubs == null)
          {
              return Problem("Entity set 'ForumsDbContext.Clubs'  is null.");
          }
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClub", new { id = club.Id }, club);
        }

        [HttpGet("User/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClubWithRoleDTO>>> GetUserClubs(int id)
        {
            if (_context.Clubs == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var clubs = await _context.UserClubs.Where(u => u.UserId == id).Include(u => u.Club)
                .OrderBy(u => u.Club.Name)
                .ThenByDescending(u => u.Club.Type)
                .Select(u => new ClubWithRoleDTO
                {
                    Club = new ClubDTO(u.Club),
                    Role = (int)u.Role
                })
                .AsNoTracking()
                .ToListAsync();

            return clubs;
        }



        [HttpPost("Join/{id}")]
        [Authorize]
        public async Task<IActionResult> JoinClub(int id)
        {
            if (_context.UserClubs == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs.FindAsync(id);

            if (club == null)
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

            var relation = await _context.UserClubs
                .Where(uc => uc.UserId == userId && uc.ClubId == id)
                .FirstOrDefaultAsync();

            if (relation != null)
            {
                return Problem("User has already joined this club!");
            }

            _context.UserClubs.Add(new UserClub
            {
                UserId = userId,
                ClubId = id,
                Role = ClubRole.Member,
                LastUpdated = DateTime.UtcNow
            });

            var activity = new Activity
            {
                Title = "Joined a Club!",
                Message = $"You have successfully joined {club.Name}!",
                Source = "Clubs",
                Date = DateTime.UtcNow,
            };

            user.Activities.Add(activity);

            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        
        [HttpDelete("Leave/{id}")]
        [Authorize]
        public async Task<IActionResult> LeaveClub(int id)
        {
            if (_context.UserClubs == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs.FindAsync(id);

            if (club == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.SubscribedForums)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            var relation = await _context.UserClubs
                .Where(uc => uc.UserId == userId && uc.ClubId == id)
                .FirstOrDefaultAsync();

            if (relation == null)
            {
                return Problem("User is not affiliated with this club.");
            }

            _context.UserClubs.Remove(relation);

            user.SubscribedForums.RemoveAll(f => f.ClubId == id && f.Visibility == ForumVisibility.Club);

            var activity = new Activity
            {
                Title = "Left a Club!",
                Message = $"You have successfully left {club.Name}!",
                Source = "Clubs",
                Date = DateTime.UtcNow,
            };

            user.Activities.Add(activity);

            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        [HttpGet("{club}/Member/{userId}/Role")]
        public async Task<ActionResult<ClubRole>> GetClubMemberRole(int club, int userId)
        {
            if (_context.UserClubs == null)
            {
                return NotFound();
            }

            var userClub = await _context.UserClubs
                .Where(uc => uc.UserId == userId && uc.ClubId == club)
                .FirstOrDefaultAsync();

            if (userClub == null)
            {
                return ClubRole.Visitor;
            }

            return userClub.Role;
        }

        [HttpPut("{club}/Member/{user}/Role/{role}")]
        [Authorize]
        public async Task<IActionResult> ChangeClubMemberRole(int club, int user, int role)
        {
            if (_context.UserClubs == null || _context.Clubs == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int requestingUserId))
            {
                return Unauthorized();
            }

            var authroized = await _clubService.IsMemberRole(requestingUserId, club, ClubRole.Leader);

            if (!authroized)
            {
                return Forbid();
            }

            var relation = await _context.UserClubs
                .Where(uc => uc.UserId == user && uc.ClubId == club)
                .FirstOrDefaultAsync();

            if (relation == null)
            {
                return Problem();
            }

            relation.Role = (ClubRole)role;

            _context.Entry(relation).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        [HttpGet("{id}/Member")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BasicUserWithClubRoleDTO>>> GetClubMembers(int id)
        {
            if (_context.UserClubs == null)
            {
                return Problem("Unable to fetch user status.");
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            return await _context.UserClubs.Where(uc => uc.ClubId == id).Include(uc => uc.User)
                .OrderBy(uc => uc.User.Name)
                .Select(uc => new BasicUserWithClubRoleDTO
                {
                    User = new BasicUserDTO(uc.User),
                    Role = (int)uc.Role,
                })
                .AsNoTracking()
                .ToListAsync();

        }

        [HttpGet("{id}/Member/Count")]
        public async Task<ActionResult<int>> GetClubMemberCount(int id)
        {
            if (_context.UserClubs == null)
            {
                return Problem("Unable to fetch user status.");
            }

            var count = await _context.UserClubs
                .Where(uc => uc.ClubId == id)
                .CountAsync();

            return count;
        }

        // DELETE: api/Clubs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            if (_context.Clubs == null)
            {
                return NotFound();
            }
            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }

            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClubExists(int id)
        {
            return (_context.Clubs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
