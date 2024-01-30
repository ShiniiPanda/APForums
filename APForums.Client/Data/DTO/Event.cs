using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class Event
    { 
#nullable enable
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
#nullable disable

        public string GetStartDate()
        {
            if (StartDate == null) return "Unknown";
            if (StartDate is DateTime)
            {
                return StartDate.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
            return "Unknown";
        }

        public string GetEndDate()
        {
            if (EndDate == null) return "Unknown";
            if (EndDate is DateTime)
            {
                return EndDate.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
            return "Unknown";
        }

        public string GenerateEventFileName(string extension)
        {
            return $"{Title}-{DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss")}";
        }

    }
}
