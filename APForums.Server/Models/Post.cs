using APForums.Server.Data.DTO;
using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("posts")]
    public class Post
    {

        public Post()
        {

        }

        public Post(PostDTO dto)
        {
            Title = dto.Title!;
            Content = dto.Content;
            Type = (PostType)dto.Type!;
            PublishedDate = DateTime.UtcNow;
            LastUpdated = DateTime.UtcNow;
            ForumId = (int)dto.ForumId!;
            UserId = dto.User.Id;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Title", TypeName = "nvarchar(512)")]
        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public PostType Type { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        // Relationship Navigators

        public List<PostTag> PostTags { get; } = new();

        public Forum Forum { get; set; } = null!;

        public int ForumId { get; set; }

        public User User { get; set; } = null!;

        public int UserId { get; set; }

        public ICollection<Comment> Comments { get; } = new List<Comment>();

        public ICollection<PostImpression> Impressions { get; } = new List<PostImpression>();

        public List<User> UsersWithImpressions { get; } = new();


    }
}
