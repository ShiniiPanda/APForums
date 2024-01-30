using APForums.Server.Controllers.Services.Data;
using APForums.Server.Controllers.Services.Interfaces;
using APForums.Server.Data;
using APForums.Server.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly ForumsDbContext _context;

        private readonly IIdentityService _identityService;

        public AdminController(ForumsDbContext forumsDbContext, IIdentityService identityService)
        {
            _context = forumsDbContext;
            _identityService = identityService;
        }

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
            Console.WriteLine("SUCCESSFUL.. RETURN VALUE IS " + authResponse.AccessToken);
            return new LoginResponse
            {
                AccessToken = authResponse.AccessToken,
                RefreshToken = authResponse.RefreshToken,
            };
        }

    }
}
