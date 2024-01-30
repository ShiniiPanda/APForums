using APForums.Client.Data.DTO;
using APForums.Client.Data.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Interfaces
{
    public interface IActivityService
    {
        Task<BasicHttpResponseWithData<PaginatedList<Activity>>> GetUserActivities(int page = 1, int size = 10, int type = 0);
        Task<BasicHttpResponse> MarkActivityAsRead(int id);
    }
}
