using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("activities")]
    public class Activity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Title", TypeName = "nvarchar(256)")]
        public string Title { get; set; } = null!;

        [Column("Source", TypeName = "nvarchar(100)")]
        public string? Source { get; set; } = null!;

        [Column("Message", TypeName = "nvarchar(512)")]
        public string Message { get; set; } = null!;

        public DateTime Date { get; set; }

        // Relationship Navigators
           
        public List<User> Users { get; } = new List<User>();

        public List<UserActivity> UserActivities { get; } = new List<UserActivity>();



    }
}
