using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{

    [Table("event_interests")]
    public class EventInterest
    {
        public int UserId { get; set; }

        public int EventId { get; set; }

        public User User { get; set; } = null!;

        public Event Event { get; set; } = null!;
    }
}
