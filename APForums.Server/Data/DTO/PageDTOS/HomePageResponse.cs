namespace APForums.Server.Data.DTO.PageDTOS
{
    public class HomePageResponse
    {

        public List<ActivityDTO> Activities { get; set; } = new();

        public List<PostDTO> Posts { get; set; } = new();

    }
}
