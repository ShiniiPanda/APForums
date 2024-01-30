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
    public interface IClubService
    {
        Task<BasicHttpResponse> ChangeClubMemberStatus(int clubId, int userId, int role);
        Task<BasicHttpResponseWithData<IEnumerable<Club>>> GetAllClubs(string search = "");
        Task<Club> GetClub(int clubId);
        Task<BasicHttpResponseWithData<IEnumerable<BasicUserWithClubRole>>> GetClubMembers(int clubId);
        Task<int> GetMemberStatus(int clubId, int userId);
        Task<BasicHttpResponseWithData<SingleClub>> GetSingleClubResponse(int id);
        Task<BasicHttpResponseWithData<IEnumerable<ClubWithRole>>> GetUserClubs(int id);
        Task<BasicHttpResponse> JoinClub(int clubId);
        Task<BasicHttpResponse> LeaveClub(int clubId);
    }
}
