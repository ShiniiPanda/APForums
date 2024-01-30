namespace APForums.Server.Data.DTO.PageDTOS
{
    public class SinglePostResponse
    {
        public PostDTO Post { get; set; } = null!;

        public List<PostTagDTO> Tags { get; set; } = new();

        public List<PostImpressionDTO> Impressions { get; set; } = new();

        public List<CommentDTO> Comments { get; set; } = new();

        public string ForumName { get; set; } = null!;
    }
}
