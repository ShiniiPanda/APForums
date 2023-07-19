using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("users_clubs")]
    public class UserClub
    {

        public int UserId { get; set; }
        public int ClubId { get; set; }

        public ClubRole Role { get; set; }

        public DateTime? LastUpdated { get; set; }

    }
}
