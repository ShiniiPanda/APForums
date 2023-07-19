using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("clubs")]
    public class Club
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name", TypeName = "nvarchar(255)")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ClubStatus Status { get; set; }

        // Relationship Navigators

        public List<User> Users { get; } = new();

        public List<UserClub> UserClubs { get; } = new();

        public ICollection<Event> Events = new List<Event>();

    }
}
