using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APForums.Server.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("TpNumber", TypeName = "nvarchar(10)")]
        public string TPNumber { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        [Column("Email", TypeName = "nvarchar(255)")]
        public string Email { get; set; } = null!;

        [Column("Phone", TypeName = "nvarchar(20)")]
        public string? PhoneNumber { get; set; }

        [Column("DOB")]
        public DateTime? DOB { get; set; }

        public Degree? DegreeType { get; set; }

        public School? Department { get; set; }

        public Course? Course { get; set; }

        public Enrollment? Enrollment { get; set; }

        public int? Level { get; set; }

        public string? IntakeCode { get; set; }

        // Relationship Navigators

        public ICollection<Social> Socials = new List<Social>(); // To access social links

        public List<Club> Clubs { get; } = new(); // To access user clubs

        public List<UserClub> UserClubs { get; } = new();

        /*public ICollection<Connection> FollowersList = new List<Connection>();

        public ICollection<Connection> FollowingList = new List<Connection>();   */


        public List<User> FollowersList { get; } = new();

        public List<User> FollowingList { get; } = new();

        public List<ProfileTag> ProfileTags = new();


        /*public string GetFirstName()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return string.Empty;
            }

            return $"{Name[0]}";
        }*/

    }
}
