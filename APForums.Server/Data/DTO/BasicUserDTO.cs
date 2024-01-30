using APForums.Server.Models;
using System.Text.Json.Serialization;

namespace APForums.Server.Data.DTO
{
    public class BasicUserDTO
    {
        [JsonConstructor]
        public BasicUserDTO() { }

        public BasicUserDTO(User user)
        {
            Id = user.Id;
            TPNumber = user.TPNumber;
            Name = user.Name;
            Intake = user.IntakeCode;
            Picture = user.Picture;
        }

        public int Id { get; set; }

        public string? TPNumber { get; set; }

        public string? Name { get; set; }

        public string? Intake { get; set; }

        public string? Picture { get; set; }

    }
}
