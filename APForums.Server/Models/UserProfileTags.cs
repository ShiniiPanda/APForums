using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("users_profile_tags")]
    public class UserProfileTags
    {

        [Column(Order = 0)]
        public int UserId { get; set; }

        [Column(Order = 1)]
        public int ProfileTagId { get; set; }

        public User User { get; set; } = null!;

        public ProfileTag ProfileTag { get; set; } = null!;

    }
}
