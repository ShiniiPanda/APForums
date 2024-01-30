using APForums.Server.Data.DTO;
using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("forums")]
    public class Forum
    {
        public Forum()
        {

        }

        public Forum(ForumDTO dto) 
        {
            Name = dto.Name!;
            Description = dto.Description;
            if (dto.Visibility == null)
            {
                Visibility = ForumVisibility.Public;
            } else
            {
                Visibility = (ForumVisibility)dto.Visibility;
            }
            Intake = dto.Intake;
            ClubId = dto.ClubId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name", TypeName = "nvarchar(256)")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ForumVisibility Visibility { get; set; }
        
        [Column(TypeName = "nvarchar(30)")]
        public string? Intake { get; set; }

        // Relationship Navigator

        public ICollection<Post> Posts { get; } = new List<Post>();

        public List<User> SubscribedUsers { get; } = new List<User>();

        public Club? Club { get; set; }

        public int? ClubId { get; set; }


    }
}
