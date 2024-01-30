using APForums.Server.Models.Types;
using APForums.Server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace APForums.Server.Data.DTO
{
    public class PostDTO
    {
        [JsonConstructor]
        public PostDTO()
        {
        }

        public PostDTO(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            Type = (int)post.Type;
            PublishedDate = post.PublishedDate;
            LastUpdated = post.LastUpdated;
            ForumId = post.ForumId;
            User.Id = post.UserId;
        }

        public PostDTO(Post post, List<PostTag> Tags)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            Type = (int)post.Type;
            PublishedDate = post.PublishedDate;
            LastUpdated = post.LastUpdated;
            foreach (var tag in Tags)
            {
                if (tag != null)
                {
                    PostTags.Add(tag.Id);
                }
            }
            ForumId = post.ForumId;
            User.Id = post.UserId;
        }

        public int Id { get; set; }

        public string? Title { get; set; } = null!;

        public string? Content { get; set; }

        public int? Type { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public List<int> PostTags { get; set; } = new();

        public int? ForumId { get; set; }

        public string? ForumName { get; set; }

        public BasicUserDTO User { get; set; } = new();
    }
}
