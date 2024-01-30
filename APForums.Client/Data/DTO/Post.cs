using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class Post
    {

#nullable enable

        public int Id { get; set; }

        public string? Title { get; set; } = null!;

        public string? Content { get; set; }

        public int Type { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public List<int> PostTags { get; set; } = new();

        public int? ForumId { get; set; }

        public string? ForumName { get; set; }

        public BasicUser? User { get; set; }

#nullable disable

        public string GetPostDate()
        {
            if (PublishedDate is DateTime)
            {
                var datetime = (DateTime)PublishedDate;
                TimeSpan difference = DateTime.UtcNow - datetime;
                if (difference.TotalDays >= 1)
                {
                    return $"{(int)difference.TotalDays} days ago";
                }
                else if (difference.TotalHours >= 1)
                {
                    return $"{(int)difference.TotalHours} hours ago";
                }
                else if (difference.TotalMinutes >= 1)
                {
                    return $"{(int)difference.TotalMinutes} minutes ago";
                }
                else
                {
                    return $"{(int)difference.TotalSeconds} seconds ago";
                }
            }
            return "some time ago";
        }

        public string GetTagName(int id)
        {
            PostTag.CachedPostTags.TryGetValue(id, out PostTag tag);
            if (tag == null) return "Unknown";
            return tag.Name;
        }

        public string GetContentPreview()
        {
            if (Content.Length > ContentPreviewLength)
            {
                return $"{Content.Substring(0, ContentPreviewLength - 1)}..";
            } else
            {
                return Content;
            }
        }

        public static int ContentPreviewLength = 80;

        public readonly static Dictionary<int, string> TypeNames = new Dictionary<int, string>()
        {
            {0, "Announcement"},
            {1, "Discussion"},
            {2, "Questions" },
            {3, "Event" },
            {4, "Meet Up" },
            {5, "Information" },
            {6, "Emergency" },
        };
    }
}
