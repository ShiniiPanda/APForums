using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class Activity
    {
#nullable enable
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Source { get; set; } = null!;

        public string Message { get; set; } = null!;

        public DateTime Date { get; set; }

        public bool Read { get; set; } = false;
#nullable disable

    }
}
