using APForums.Server.Models.Types;

namespace APForums.Server.Controllers.Services.Interfaces
{
    public interface IClubService
    {
        Task<bool> IsMember(int memberId, int clubId);
        Task<bool> IsMemberCommittee(int memberId, int clubId);
        Task<bool> IsMemberRole(int memberId, int clubId, ClubRole role);
    }
}
