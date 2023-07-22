using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("forums")]
    public class Forum
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        
        [Column(TypeName = "nvarchar(30)")]
        public string? Intake { get; set; }

        // Relationship Navigator

        public ICollection<Post> Posts { get; } = new List<Post>();


    }
}
