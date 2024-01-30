using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APForums.Server.Models;

namespace APForums.Server.Data.DTO
{
    public class EventDTO
    {
        [JsonConstructor]
        public EventDTO() { }

        public EventDTO(Event @event)
        {
            Id = @event.Id;
            Title = @event.Title;
            SubTitle = @event.SubTitle;
            Description = @event.Description;
            ImagePath = @event.ImagePath;
            PostedDate = @event.PostedDate;
            StartDate = @event.StartDate;
            EndDate = @event.EndDate;
            ClubId = @event.ClubId;
            Visibility = (int)@event.Visibility;
            if (@event.Club != null) {
                ClubName = @event.Club.Name;
            } else
            {
                ClubName = null;
            }
             
        }

        public int? Id { get; set; }

        public string? Title { get; set; } = null!;

        public string? SubTitle { get; set; }

        public string? Description { get; set; }

        public string? ImagePath { get; set; }

        public DateTime? PostedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Visibility { get; set; }

        public int? ClubId { get; set; }

        public string? ClubName { get; set; }

    }
}
