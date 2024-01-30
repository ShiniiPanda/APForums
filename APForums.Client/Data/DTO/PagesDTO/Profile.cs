using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO.PagesDTO
{
    public class Profile
    { 
        public User User { get; set; }

        public List<Club> Clubs { get; set; }

        public List<ProfileTag> ProfileTags { get; set; }

        public List<SocialLink> Socials { get; set; }

        public bool IsFollower { get; set; } = false;
        public bool IsFollowed { get; set; } = false;

        public int NumberOfFollowers { get; set; } = 0;

        public int NumberOfFollowing { get; set; } = 0;

    }
}
