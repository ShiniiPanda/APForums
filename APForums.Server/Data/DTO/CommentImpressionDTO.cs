using APForums.Server.Models;
using APForums.Server.Models.Types;
using System.Text.Json.Serialization;

namespace APForums.Server.Data.DTO
{
    public class CommentImpressionDTO
    {
        [JsonConstructor]
        public CommentImpressionDTO()
        {

        }

        public CommentImpressionDTO(CommentImpression commentImpression)
        {
            Value = (int)commentImpression.Value;
            UserId = commentImpression.UserId;
            CommentId = commentImpression.CommentId;
        }
        public int Value { get; set; }

        public int UserId { get; set; }

        public int CommentId { get; set; }

    }
}
