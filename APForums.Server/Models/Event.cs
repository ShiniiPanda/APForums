using APForums.Server.Data.DTO;
using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("events")]
    public class Event
    {

        public Event()
        {

        }

        public Event(EventDTO dto)
        {
            Title = dto.Title!;
            SubTitle = dto.SubTitle!;
            Description = dto.Description!;
            ImagePath = dto.ImagePath!; 
            PostedDate = dto.PostedDate;
            StartDate = dto.StartDate;
            EndDate = dto.EndDate;
            ClubId = dto.ClubId;
            Visibility = (EventVisibility)dto.Visibility;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Title", TypeName = "nvarchar(255)")]
        public string Title { get; set; } = null!;

        public string? SubTitle { get; set; }

        public string? Description { get; set; }

        public EventVisibility Visibility { get; set; } = EventVisibility.Public;

        public string? ImagePath { get; set; }

        public DateTime? PostedDate { get; set; }
            
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set;}

        // Relationship Navigators

        public int? ClubId { get; set; }

        public Club? Club { get; set; }

        public List<User> InterestedUsers { get; set; } = new();

        public List<EventInterest> EventInterests { get; } = new();

    }
}
