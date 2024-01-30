using Microsoft.Identity.Client;

namespace APForums.Server.Data.DTO.PageDTOS
{
    public class ProfileResponse
    {

        public UserDTO User { get; set; } = null!;
        public List<ProfileTagDTO> ProfileTags { get; set; } = new List<ProfileTagDTO>();
        public List<SocialDTO> Socials { get; set; } = new List<SocialDTO>();
        public List<ClubDTO> Clubs { get; set; } = new List<ClubDTO>();

        public bool IsFollowed { get; set; } = false;

        public bool IsFollower { get; set; } = false;

        public int NumberOfFollowers { get; set; } = 0;

        public int NumberOfFollowing { get; set; } = 0;
    }
}
