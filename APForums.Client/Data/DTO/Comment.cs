using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class Comment
    {
#nullable enable
        public int Id { get; set; }

        public string? Content { get; set; } = null!;

        public DateTime? PostedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public bool? Visible { get; set; } = true;

        public int PostId { get; set; }

        public BasicUser User { get; set; } = null!;

        public List<CommentImpression> Impressions { get; set; } = new List<CommentImpression>();
#nullable disable

        public string GetCommentDate()
        {
            if (PostedDate is DateTime)
            {
                var datetime = (DateTime)PostedDate;
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

    }
}
