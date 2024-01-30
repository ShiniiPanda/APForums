using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APForums.Server.Data;
using APForums.Server.Models;
using System.Drawing;
using APForums.Server.Data.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using APForums.Server.Controllers.Services.Data;
using APForums.Server.Controllers.Services.Interfaces;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ForumsDbContext _context;

        private readonly IConfiguration _config;

        private readonly IIdentityService _identityService;

        public UsersController(ForumsDbContext context, IConfiguration config, IIdentityService identityService)
        {
            _context = context;
            _config = config;
            _identityService = identityService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.Select(u => new UserDTO(u)).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDTO(user);

            return userDto;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null || _context == null)
            {
                return Problem("Entity set 'ForumsDbContext.Users'  is null.");
            }

            /*var testUser = new User
            { 
                Id =  6,
                Name = "TestPanda",
                Password = "iloveyou",
                TPNumber = "TP0445126",
                Email = "testpanda@gmail.com"
            };*/

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        /*
         * 
         * User Endpoints for authentication and token validation/refresh (JWT).
         * 
         */


        [HttpPost("Authenticate")]
        public async Task<ActionResult<LoginResponse>> Authenticate(LoginRequest loginRequest)
        {

            if (loginRequest == null)
            {
                return BadRequest();
            }

            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.TPNumber == loginRequest.Username);


            if (user == null)
            {
                return NotFound();
            }


            var validateResult = user.validatePassword(loginRequest.Password);

            if (validateResult == false)
            {
                return NotFound();
            }

            var authResponse = await _identityService.GenerateTokenForValidatedUser(user.Id, user.TPNumber);
            if (authResponse.Status != AuthStatus.Successful)
            {
                return BadRequest();
            }
            return new LoginResponse
            {
                AccessToken = authResponse.AccessToken,
                RefreshToken = authResponse.RefreshToken,
            };
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult<LoginResponse>> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {

            if (refreshTokenRequest == null)
            {
                return BadRequest();
            }

            var authResponse = await _identityService.GetRefreshAsync(refreshTokenRequest);

            if (authResponse.Status != AuthStatus.Successful)
            {
                return BadRequest();
            }

            Console.WriteLine("SUCCESSFUL.. RETURN VALUE IS " + authResponse.AccessToken);
            return new LoginResponse
            {
                AccessToken = authResponse.AccessToken,
                RefreshToken = authResponse.RefreshToken
            };
        }


        /*
         * 
         * User Endpoints for manipulation profile metrics and parameters.
         * 
         */

        [HttpPut("{id}/Picture")]
        [Authorize]
        public async Task<IActionResult> UpdatePicture(int id, [FromBody] string picture)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            if (id != userId || string.IsNullOrWhiteSpace(picture))
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.Picture = picture;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost("Follow/{id}")]
        [Authorize]
        public async Task<IActionResult> Follow(int id)
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

            var targetUser = await _context.Users.FindAsync(id);

            if (targetUser == null)
            {
                return NotFound();
            }

            if (_context.Connections.Where(c => c.FollowerId == userId && c.FollowedId == id).Any())
            {
                return Problem("Connection already exists between those users!");
            }

            var relation = new Connection
            {
                FollowerId = userId,
                FollowedId = id,
                Date = DateTime.UtcNow
            };

            _context.Connections.Add(relation);

            var activity = new Activity
            {

                Title = "Followed a Friend!",
                Message = $"You have started following {targetUser.Name}!",
                Source = "Connections",
                Date = DateTime.UtcNow
            };

            user.Activities.Add(activity);

            await _context.SaveChangesAsync();

            /*return CreatedAtAction("GetUser", new { id = user.Id }, user);*/
            return Ok("Success!");
        }


        [Authorize]
        [HttpDelete("Unfollow/{id}")]
        public async Task<IActionResult> Unfollow(int id)
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

            var targetUser = await _context.Users.FindAsync(id);

            if (targetUser == null)
            {
                return NotFound();
            }

            var relation = await _context.Connections.Where(c => c.FollowerId == userId && c.FollowedId == id).FirstOrDefaultAsync();

            if (relation == null)
            {
                return Problem("Connectio already does not exist between both users!");
            }

            _context.Connections.Remove(relation);

            var activity = new Activity
            {

                Title = "Unfollowed a Friend!",
                Message = $"You have started following {targetUser.Name}!",
                Source = "Connections",
                Date = DateTime.UtcNow
            };

            user.Activities.Add(activity);

            await _context.SaveChangesAsync();

            /*return CreatedAtAction("GetUser", new { id = user.Id }, user);*/
            return Ok("Success!");
        }


        [HttpGet("{id}/Followers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BasicUserDTO>>> Followers(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var users = await _context.Connections.Where(c => c.FollowedId == id)
                .Include(c => c.Follower)
                .OrderBy(c => c.Follower.Name)
                .Select(c => new BasicUserDTO(c.Follower))
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        [HttpGet("{id}/Following")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BasicUserDTO>>> Following(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var users = await _context.Connections
                .Where(c => c.FollowerId == id)
                .Include(c => c.Followed)
                .OrderBy(c => c.Followed.Name)
                .Select(c => new BasicUserDTO(c.Followed))
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /*private async Task<TokenValidationResult?> validateToken(string token)
        {
            if (_config == null || !_config.GetSection("JWTSecret").Exists())
            {
                return null;
            }
            var key = Encoding.ASCII.GetBytes(_config.GetSection("JWTSecret").Value!);
            var signingKey = new SymmetricSecurityKey(key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            return await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
        }*/
    }
}
