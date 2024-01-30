namespace APForums.Server.Data.DTO.PageDTOS
{
    public class SingleClubResponse
    {

        public ClubDTO Club { get; set; } = null!;

        public List<ForumDTO> Forums { get; set; } = null!;

        public List<EventWIthInterestDTO> Events { get; set; } = null!;

        public int MemberCount { get; set; } = 0;

        public int Role { get; set; } = 0;


    }
}
