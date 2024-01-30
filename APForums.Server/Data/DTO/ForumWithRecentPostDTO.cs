using APForums.Server.Models;

namespace APForums.Server.Data.DTO
{
    public class ForumWithRecentPostDTO
    {

        public ForumDTO Forum { get; set; } = null!;

        public PostDTO? Post { get; set; }

    }
}
