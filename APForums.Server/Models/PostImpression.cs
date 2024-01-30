using APForums.Server.Data.DTO;
using APForums.Server.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APForums.Server.Models
{
    [Table("post_impressions")]
    public class PostImpression
    {

        [JsonConstructor]
        public PostImpression() { }

        public PostImpression(PostImpressionDTO dto)
        {
            Value = (ImpressionValue)dto.Value;
            LastUpdated = DateTime.UtcNow;
            UserId = dto.UserId;
            PostId = dto.PostId;
        }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public ImpressionValue Value { get; set; }

        public DateTime? LastUpdated { get; set; }

        public User User { get; set; } = null!;

        

        public Post Post { get; set; } = null!;


    }
}
