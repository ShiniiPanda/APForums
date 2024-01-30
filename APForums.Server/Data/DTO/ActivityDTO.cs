using APForums.Server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APForums.Server.Data.DTO
{
    public class ActivityDTO
    {
        [JsonConstructor]
        public ActivityDTO()
        {

        }


        public ActivityDTO(Activity activity)
        {
            Id = activity.Id;
            Title = activity.Title;
            Source = activity.Source;
            Message = activity.Message;
            Date = activity.Date;
        }

        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Source { get; set; } = null!;

        public string Message { get; set; } = null!;

        public DateTime Date { get; set; }

        public bool Read { get; set; } = false;

    }
}
