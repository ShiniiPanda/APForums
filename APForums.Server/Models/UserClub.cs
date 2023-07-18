using APForums.Server.Models.Types;

namespace APForums.Server.Models
{
    public class UserClub
    {

        public int UserId { get; set; }
        public int ClubId { get; set; }

        public ClubRole Role { get; set; }

        public DateTime? LastUpdated { get; set; }

    }
}
