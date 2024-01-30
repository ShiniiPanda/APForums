using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("refresh_tokens")]
    public class RefreshToken
    {

        [Key]
        public string Token { get; set; } = null!;

        public string JwtId { get; set; } = null!;

        public DateTime? Created { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool Used { get; set; } = false;

        public bool Invalid { get; set; } = false;

        //Relationship Navigators

        public int? UserId { get; set; }

        public User? User { get; set; }

    }
}
