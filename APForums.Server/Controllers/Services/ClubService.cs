using APForums.Server.Controllers.Services.Interfaces;
using APForums.Server.Data;
using APForums.Server.Models.Types;
using Microsoft.EntityFrameworkCore;

namespace APForums.Server.Controllers.Services
{
    public class ClubService : IClubService
    {

        private readonly ForumsDbContext _context;

        public static int CommitteeRoleThreshold = (int)ClubRole.Commitee;

        public ClubService(ForumsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsMemberCommittee(int memberId, int clubId)
        {
            var relation = await _context.UserClubs
                .Where(uc => uc.UserId == memberId && uc.ClubId == clubId)
                .FirstOrDefaultAsync();

            if (relation == null)
            {
                return false;
            }

            if ((int)relation.Role < CommitteeRoleThreshold)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsMemberRole(int memberId, int clubId, ClubRole role)
        {
            var relation = await _context.UserClubs
                .Where(uc => uc.UserId == memberId && uc.ClubId == clubId)
                .FirstOrDefaultAsync();

            if (relation == null)
            {
                return false;
            }

            if (relation.Role == role)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsMember(int memberId, int clubId)
        {
            var relation = await _context.UserClubs
                .Where(uc => uc.UserId == memberId && uc.ClubId == clubId)
                .FirstOrDefaultAsync();

            if (relation == null)
            {
                return false;
            }

            return true;
        }
    }
}
