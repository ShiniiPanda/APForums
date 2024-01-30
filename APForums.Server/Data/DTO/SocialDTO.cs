using APForums.Server.Models;
using APForums.Server.Models.Types;
using System.Text.Json.Serialization;

namespace APForums.Server.Data.DTO
{
    public class SocialDTO
    {
        [JsonConstructor]
        public SocialDTO() { }

        public SocialDTO(Social social)
        {
            Id = social.Id;
            Value = social.Value;
            Type = (int)social.Type;
            LastUpdated = social.LastUpdated;
            UserId = social.UserId;
        }

        public int Id { get; set; }

        public string Value { get; set; } = null!;

        public int Type { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int UserId { get; set; }

    }
}
