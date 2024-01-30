using APForums.Client.Data.DTO;
using APForums.Client.Data.DTO.PagesDTO;
using APForums.Client.Data.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Interfaces
{
    public interface IEventService
    {
        Task<BasicHttpResponseWithData<int>> AddClubEvent(int clubId, Event @event);
        Task<BasicHttpResponse> AddEventInterest(int id);
        Task<BasicHttpResponseWithData<IEnumerable<BasicUser>>> GetEventInterestedUsers(int id);
        Task<BasicHttpResponseWithData<EventWithInterest>> GetSingleEventWithInterest(int id);
        Task<BasicHttpResponseWithData<UserEvents>> GetUserEvents(int id);
        Task<BasicHttpResponse> RemoveEventInterest(int id);
    }
}
