using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APForums.Server.Models;

namespace APForums.Server.Data.DTO
{
    public class ProfileTagDTO
    {
        [JsonConstructor]
        public ProfileTagDTO() { }

        public ProfileTagDTO(ProfileTag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            FilePath = tag.FilePath;
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? FilePath { get; set; }

    }
}
