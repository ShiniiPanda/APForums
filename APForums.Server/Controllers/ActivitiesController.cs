using APForums.Server.Controllers.Services.Interfaces;
using APForums.Server.Data;
using APForums.Server.Data.DTO;
using APForums.Server.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {

        private readonly ForumsDbContext _context;

        private readonly IIdentityService _identityService;

        public ActivitiesController(ForumsDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserActivities(int page = 0, int size = 0, int type = 0)
        {
            if (_context.Activities == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var baseQuery = _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .Include(ua => ua.Activity).AsQueryable();

            switch (type)
            {
                case 1:
                    baseQuery = baseQuery.Where(ua => ua.Read == false).OrderByDescending(ua => ua.Activity.Date);
                    break;
                case 2:
                    baseQuery = baseQuery.Where(ua => ua.Read == true).OrderByDescending(ua => ua.Activity.Date);
                    break;
                default:
                    baseQuery = baseQuery.OrderBy(ua => ua.Read).ThenByDescending(ua => ua.Activity.Date);
                    break;
            }

            if (page == 0)
            {
                List<ActivityDTO> activityList = new();

                if (size == 0)
                {
                    activityList = await baseQuery.Select(ua => new ActivityDTO(ua.Activity) { Read = ua.Read })
                    .AsNoTracking()
                    .ToListAsync();
                } else
                {
                    activityList = await baseQuery.Take(size).Select(ua => new ActivityDTO(ua.Activity) { Read = ua.Read })
                    .AsNoTracking()
                    .ToListAsync();
                }

                return Ok(activityList);
            }

            var activities = await baseQuery.Skip((page - 1) * size).Take(size)
                .Select(ua => new ActivityDTO(ua.Activity) { Read = ua.Read })
                .AsNoTracking()
                .ToListAsync();

            var totalActivityCount = baseQuery.Count();
            var totalPageCount = (int)Math.Ceiling((double)totalActivityCount / size);

            var response = new PaginatedListDTO<ActivityDTO>()
            {
                Items = activities,
                CurrentPage = page,
                PageSize = size,
                TotalPages = totalPageCount,
                TotalItems = totalActivityCount
            };

            return Ok(response);
        }

        [HttpPut("Read/{id}")]
        [Authorize]
        public async Task<IActionResult> MarkActivityRead(int id)
        {
            if (_context.Activities == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var activity = await _context.UserActivities.Where(ua => ua.UserId == userId && ua.ActivityId == id).FirstOrDefaultAsync();

            if (activity == null)
            {
                return NotFound();
            }

            activity.Read = true;
            _context.Entry(activity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("Read")]
        [Authorize]
        public async Task<IActionResult> MarkAllActivityRead()
        {
            if (_context.Activities == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            await _context.UserActivities.Where(ua => ua.UserId == userId && ua.Read == false)
                .ExecuteUpdateAsync(s => s.SetProperty(ua => ua.Read, true));

            return Ok();
        }
    }
}
