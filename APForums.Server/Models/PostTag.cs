using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("post_tags")]
    public class PostTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Column("FilePath", TypeName = "nvarchar(500)")]
        public string? FilePath { get; set; }

        // Relationship Navigators

        public List<Post> Posts { get; } = new();



    }
}
