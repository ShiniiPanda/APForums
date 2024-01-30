using APForums.Server.Data.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APForums.Server.Models
{
    [Table("comments")]
    public class Comment
    {
        [JsonConstructor]
        public Comment()
        {

        }

        public Comment(CommentDTO dto)
        {
            Content = dto.Content == null ? "" : dto.Content;
            PostedDate = DateTime.UtcNow;
            LastUpdated = DateTime.UtcNow;
            Visible = dto.Visible;
            UserId = dto.User.Id;
            PostId = dto.PostId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime? PostedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public bool Visible { get; set; } = true;

        // Relationship Navigators

        public Post Post { get; set; } = null!;
         
        public int PostId { get; set; }

        public User User { get; set; } = null!;

        public int UserId { get; set; }

        public ICollection<CommentImpression> Impressions { get; } = new List<CommentImpression>();

        public List<User> UsersWithImpressions { get; } = new List<User>();

    }
}
