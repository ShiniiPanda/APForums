using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class BasicUser
    {
#nullable enable
        public int? Id { get; set; }

        public string? TPNumber { get; set; }

        public string? Name { get; set; }

        public string? Intake { get; set; }

        public string? Picture { get; set; }
#nullable disable
    }
}
