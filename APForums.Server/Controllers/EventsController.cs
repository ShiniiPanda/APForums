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
using Microsoft.AspNetCore.Authorization;
using APForums.Server.Controllers.Services.Interfaces;
using System.Text.Json;

namespace APForums.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ForumsDbContext _context;

        private readonly IClubService _clubService;

        private readonly IIdentityService _identityService;

        public EventsController(ForumsDbContext context, IClubService clubService, IIdentityService identityService)
        {
            _context = context;
            _clubService = clubService;
            _identityService = identityService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents([FromQuery] int club = -1)
        {
          if (_context.Events == null)
          {
              return NotFound();
          }

          var events = new List<EventDTO>();

          if (club == -1)
            {

                events = await _context.Events.Select(e => new EventDTO(e)).ToListAsync();
            } 
            else
            {
                events = await _context.Events.Where(e => e.ClubId == club).Select(e => new EventDTO(e)).ToListAsync();
            }

            return events;
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
          if (_context.Events == null)
          {
              return NotFound();
          }
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return new EventDTO(@event);
        }

        // GET: api/Events/5
        [HttpGet("Interest/Event/{id}")]
        [Authorize]
        public async Task<ActionResult<EventWIthInterestDTO>> GetSingleEventWithInterest(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }

            if(!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var @event = await _context.Events.Where(e => e.Id == id)
                .GroupJoin(
                _context.EventInterests.Where(ei => ei.UserId == userId),
                e => e.Id,
                i => i.EventId,
                (e, i) => new EventWIthInterestDTO
                {
                    Event = new EventDTO(e) { ClubName = e.Club == null ? "APU" : e.Club.Name },
                    IsInterested = i.Any()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // GET: api/Events/5
        [HttpGet("Interest/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BasicUserDTO>>> GetInterestedUsers(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var users = await _context.EventInterests
                .Where(ei => ei.EventId == id)
                .Select(u => new BasicUserDTO(u.User))
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        // GET: api/Events/5
        [HttpPost("Interest/Add/{id}")]
        [Authorize]
        public async Task<IActionResult> AddInterest(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var IsInterested = _context.EventInterests.Where(ei => ei.UserId == userId && ei.EventId == id).Any();
            if (IsInterested)
            {
                return Problem("User is already interested in this particular event!");
            }

            var interest = new EventInterest {
            
                UserId = userId,
                EventId = id,

            };

            _context.EventInterests.Add(interest);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/Events/5
        [HttpDelete("Interest/Remove/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BasicUserDTO>>> RemoveInterest(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidUserFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var interest = await _context.EventInterests.Where(ei => ei.UserId == userId && ei.EventId == id).FirstOrDefaultAsync();

            if (interest == null)
            {
                return Problem("User is not interested in this particular event!");
            }

            _context.EventInterests.Remove(interest);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/Events/5
        [HttpGet("User")]
        [Authorize]
        public async Task<IActionResult> GetUserEvents()
        {
            if (_context.Events == null)
            {
                return NotFound();
            }

            if (!_identityService.IsValidIdFromClaim(HttpContext.User, out int userId))
            {
                return Unauthorized();
            }

            var events = await _context.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.Clubs)
                .SelectMany(c => c.Events.Where(e => e.Visibility == Models.Types.EventVisibility.Private), (c, e) => new { ClubName = c.Name, ClubEvent = e })
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
                .ToListAsync();

            return Ok(events);
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
          if (_context.Events == null)
          {
              return Problem("Entity set 'ForumsDbContext.Events'  is null.");
          }
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Club/{id}")]
        [Authorize]
        public async Task<IActionResult> PostClubEvent(int id, EventDTO @event)
        {
            if (_context.Events == null)
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

            var clubName = await _context.Clubs.Where(c => c.Id == id).Select(c => c.Name).FirstOrDefaultAsync();

            if (clubName == null)
            {
                return NotFound();
            }

            var IsCommitee = await _clubService.IsMemberCommittee(userId, id);

            if (!IsCommitee)
            {
                return Forbid();
            }

            var tracker = _context.Events.Add(new Event(@event));

            var activity = new Activity
            {

                Title = "Added new Event!",
                Message = $"You have added a new event under {clubName}",
                Date = DateTime.Now,
                Source = "Events"
            };

            user.Activities.Add(activity);

            await _context.SaveChangesAsync();

            return Ok(tracker.Entity.Id);
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Club/{clubId}/{eventId}")]
        [Authorize]
        public async Task<ActionResult<Event>> UpdateClubEvent(EventDTO @event, int clubId, int eventId)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }

            if (eventId != @event.Id)
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

            var eventModel = new Event(@event);
            eventModel.Id = (int)@event.Id;

            _context.Entry(eventModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventId))
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

        [HttpDelete("Club/{clubId}/{eventId}")]
        [Authorize]
        public async Task<ActionResult<Event>> DeleteClubEvent(int clubId, int eventId)
        {
            if (_context.Events == null)
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

            var clubName = await _context.Clubs.Where(c => c.Id == clubId).Select(c => c.Name).FirstOrDefaultAsync();

            var IsCommitee = await _clubService.IsMemberCommittee(userId, clubId);

            if (!IsCommitee)
            {
                return Forbid();
            }

            var @event = await _context.Events.FindAsync(eventId);

            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);

            var activity = new Activity
            {
                Title = "Removed an Event!",
                Message = $"You have removed an event under {clubName}"
            };

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool EventExists(int id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
