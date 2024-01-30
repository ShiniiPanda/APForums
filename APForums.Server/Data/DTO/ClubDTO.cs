using APForums.Server.Models;
using System.Text.Json.Serialization;

namespace APForums.Server.Data.DTO
{
    public class ClubDTO
    {
        [JsonConstructor]
        public ClubDTO() { }

        public ClubDTO(Club club)
        {
            Id = club.Id;
            Name = club.Name;
            Abbreviation = club.Abbreviation;
            Description = club.Description;
            Status = club.Status.ToString();
            Type = (int)club.Type;
            Logo = club.LogoPath;
        }

        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Abbreviation { get; set; } = null!;

        public string? Description { get; set; }

        public string? Status { get; set; }

        public int? Type { get; set; }

        public string? Logo { get; set; }

    }
}
