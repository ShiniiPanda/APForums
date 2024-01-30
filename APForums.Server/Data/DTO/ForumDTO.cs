using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APForums.Server.Models;
using System.Text.Json.Serialization;

namespace APForums.Server.Data.DTO
{
    public class ForumDTO
    {
        [JsonConstructor]
        public ForumDTO()
        {

        }

        public ForumDTO(Forum forum)
        {
            Id = forum.Id;
            Name = forum.Name;
            Description = forum.Description;
            Visibility = (int)(forum.Visibility);
            Intake = forum.Intake;
            ClubId = forum.ClubId;
        }

        public int? Id { get; set; }

        public string? Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? Visibility { get; set; }

        public string? Intake { get; set; }

        public int? ClubId { get; set; }

        public string? ClubName { get; set; }

    }
}
