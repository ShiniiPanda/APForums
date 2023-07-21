using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    public class ProfileTag
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Column("FilePath", TypeName = "nvarchar(max)")]
        public string? FilePath { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        // Relationship Navigators 

        public List<User> Users { get; set; } = new();


    }
}
