namespace APForums.Server.Data.DTO.PageDTOS
{
    public class SocialTagResponse
    {
        public ProfileTagDTO Tag { get; set; } = null!;

        public List<UserConnectionDTO> Users { get; set; } = new List<UserConnectionDTO>();

        public bool Subscribed { get; set; } = false;

    }

    public class UserConnectionDTO
    {

        public BasicUserDTO User { get; set; } = null!;

        // 0 Means they are not connected. 1 Means they are in users's followings. 2 Means they follow the user.
        public int Connection { get; set; } = 0;

    }


}
