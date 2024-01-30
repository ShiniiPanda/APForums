using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

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

        private string _password = null!;

        public string Password {
            get => _password;
            set
            {
                using (var random = RandomNumberGenerator.Create())
                using (var newSHA256 =  SHA256.Create())
                {
                    var salt = new byte[MAX_SALT_LENGTH];
                    string saltedPass = String.Empty;
                    random.GetNonZeroBytes(salt);
                    if (salt != null)
                    {
                        HashSalt = Encoding.Default.GetString(salt);
                    }
                    saltedPass = value + HashSalt;
                    byte[] hashedBytes = newSHA256.ComputeHash(Encoding.UTF8.GetBytes(saltedPass));
                    StringBuilder hashedPass = new StringBuilder();
                    for (int i = 0; i < hashedBytes.Length; i++)
                    {
                        hashedPass.Append(hashedBytes[i].ToString("x2"));
                    }
                    _password = hashedPass.ToString();
                }
            }
        }

        [JsonIgnore]
        public string HashSalt { get; set; } = null!;

        public string Name { get; set; } = null!;

        [Column("Email", TypeName = "nvarchar(255)")]
        public string Email { get; set; } = null!;

        [Column("Phone", TypeName = "nvarchar(20)")]
        public string? PhoneNumber { get; set; }

        [Column("Picture", TypeName = "nvarchar(256)")]
        public string? Picture { get; set; }

        [Column("DOB")]
        public DateTime? DOB { get; set; }

        public Degree? DegreeType { get; set; }

        public School? Department { get; set; }

        public Course? Course { get; set; }

        public Enrollment? Enrollment { get; set; }

        public int? Level { get; set; }

        [Column("Intake", TypeName = "nvarchar(30)")]
        public string? IntakeCode { get; set; }

        // Relationship Navigators

        public ICollection<Social> Socials { get; } = new List<Social>(); // To access social links

        public List<Club> Clubs { get; } = new(); // To access user clubs

        public List<UserClub> UserClubs { get; } = new();

        /*public ICollection<Connection> FollowersList = new List<Connection>();

        public ICollection<Connection> FollowingList = new List<Connection>();   */


        public List<User> FollowersList { get; } = new();

        public List<User> FollowingList { get; } = new();

        public List<ProfileTag> ProfileTags { get;  } = new();

        public List<UserProfileTags> UserProfileTags { get; } = new();

        public List<Event> Events { get; } = new();

        public List<EventInterest> EventInterests { get; } = new();

        public ICollection<Post> UserPosts { get; } = new List<Post>(); // To access all user posts

        public ICollection<Comment> UserComments { get; } = new List<Comment>(); // To access all user comments

        public ICollection<PostImpression> PostImpressions { get; } = new List<PostImpression>(); // To access all Post Impressions

        public ICollection<CommentImpression> CommentImpressions { get; } = new List<CommentImpression>(); // To access all Comment Impressions

        public List<Activity> Activities { get; } = new List<Activity>();

        public List<UserActivity> UserActivities { get; } = new List<UserActivity>();

        public List<Forum> SubscribedForums { get; } = new List<Forum>();

        public List<Post> PostsWithImpressions { get; } = new List<Post>();

        public List<Comment> CommentsWithImpressions { get; } = new List<Comment>();

        /*public string GetFirstName()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return string.Empty;
            }

            return $"{Name[0]}";
        }*/

        // Business Logic

        private static int MAX_SALT_LENGTH = 32;

        public bool validatePassword(string password)
        {
            using (var newSHA256  = SHA256.Create())
            {
                byte[] hashedBytes = newSHA256.ComputeHash(Encoding.UTF8.GetBytes(password + HashSalt));
                StringBuilder hashedPass = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    hashedPass.Append(hashedBytes[i].ToString("x2"));
                }
                if (Password.Equals(hashedPass.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
