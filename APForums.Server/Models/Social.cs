using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("socials")]
    public class Social
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Value { get; set; } = null!;

        public SocialLink Type { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public DateTime? LastUpdated { get; set; }
    }
}
