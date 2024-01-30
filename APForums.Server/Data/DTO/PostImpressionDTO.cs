using APForums.Server.Models.Types;
using APForums.Server.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APForums.Server.Data.DTO
{
    public class PostImpressionDTO
    {
        [JsonConstructor]
        public PostImpressionDTO() { }

        public PostImpressionDTO(PostImpression impression)
        {
            Value = (int)impression.Value;
            UserId = impression.UserId;
            PostId = impression.PostId;
        }

        public int Id { get; set; }

        public int Value { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

    }
}
