using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("events")]
    public class Event
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Title", TypeName = "nvarchar(255)")]
        public string Title { get; set; } = null!;

        public string? SubTitle { get; set; }

        public string? Description { get; set; }

        public string? ImagePath { get; set; }

        public DateTime? PostedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set;}

        // Relationship Navigators

        public int? ClubId { get; set; }

        public Club? Club { get; set; }



    }
}
