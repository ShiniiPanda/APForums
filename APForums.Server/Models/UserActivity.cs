using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("users_activities")]
    public class UserActivity
    {

        public int UserId { get; set; }

        public int ActivityId { get; set; }

        public bool Read { get; set; } = false;

        public Activity Activity { get; set; } = null!;

        public User User { get; set; } = null!;

    }
}
