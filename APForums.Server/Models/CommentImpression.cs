using APForums.Server.Data.DTO;
using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APForums.Server.Models
{
    [Table("comment_impressions")]
    public class CommentImpression
    {
        [JsonConstructor]
        public CommentImpression()
        {

        }

        public CommentImpression(CommentImpressionDTO dto)
        {
            UserId = dto.UserId;
            CommentId = dto.CommentId;
            Value = (ImpressionValue)dto.Value;
        }

        public int UserId { get; set; }

        public int CommentId { get; set; }

        public ImpressionValue Value { get; set; }

        public DateTime? LastUpdated { get; set; }

        // Relationship Navigator

        public User User { get; set; } = null!;

        public Comment Comment { get; set; } = null!;

    }
}
