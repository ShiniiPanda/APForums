using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("posts")]
    public class Post
    {

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


    }
}
