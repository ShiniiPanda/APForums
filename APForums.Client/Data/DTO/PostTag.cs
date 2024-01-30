using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class PostTag
    {
#nullable enable
        public int Id { get; set; }

        public string? Name { get; set; } = null!;

        public string? FilePath { get; set; }
#nullable disable

        public static Dictionary<int, PostTag> CachedPostTags = new Dictionary<int, PostTag>()
        {
            { 1, new PostTag { Id = 1, Name = "Advice" } },
            { 2, new PostTag { Id = 2, Name = "Am I Right?" } },
            { 3, new PostTag { Id = 3, Name = "Academics" } },
            { 4, new PostTag { Id = 4, Name = "Art" } },
            { 5, new PostTag { Id = 5, Name = "Confused" } },
            { 6, new PostTag { Id = 6, Name = "Debate" } },
            { 7, new PostTag { Id = 7, Name = "Discussion" } },
            { 8, new PostTag { Id = 8, Name = "Event" } },
            { 9, new PostTag { Id = 9, Name = "Feedback" } },
            { 10, new PostTag { Id = 10, Name = "Food" } },
            { 11, new PostTag { Id = 11, Name = "Funny" } },
            { 12, new PostTag { Id = 12, Name = "Gaming" } },
            { 13, new PostTag { Id = 13, Name = "Health" } },
            { 14, new PostTag { Id = 14, Name = "Help Needed!" } },
            { 15, new PostTag { Id = 15, Name = "Inspiration" } },
            { 16, new PostTag { Id = 16, Name = "Maths" } },
            { 17, new PostTag { Id = 17, Name = "Music" } },
            { 18, new PostTag { Id = 18, Name = "News" } },
            { 19, new PostTag { Id = 19, Name = "Opinion" } },
            { 20, new PostTag { Id = 20, Name = "Vote" } },
            { 21, new PostTag { Id = 21, Name = "Question" } },
            { 22, new PostTag { Id = 22, Name = "Review" } },
            { 23, new PostTag { Id = 23, Name = "Science" } },
            { 24, new PostTag { Id = 24, Name = "Showcase" } },
            { 25, new PostTag { Id = 25, Name = "Sports" } },
            { 26, new PostTag { Id = 26, Name = "Story" } },
            { 27, new PostTag { Id = 27, Name = "Suggestion" } },
            { 28, new PostTag { Id = 28, Name = "Support" } },
            { 29, new PostTag { Id = 29, Name = "Technology" } },
            { 30, new PostTag { Id = 30, Name = "Travel" } },
            { 31, new PostTag { Id = 31, Name = "Tutorial" } },

        };
    }
}
