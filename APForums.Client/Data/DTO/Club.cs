using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class Club
    {

#nullable enable
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Abbreviation { get; set; } = null!;

        public string? Description { get; set; }

        public string? Status { get; set; }

        public int Type { get; set; }

        public string? Logo { get; set; }

        public int? Role { get; set; }
#nullable disable

        public readonly static Dictionary<int, string> ClubRoles = new()
        {
            {1, "Member" },
            {2, "Commitee" },
            {3, "Leader" },
        };

        public readonly static Dictionary<int, string> ClubTypes = new()
        {
            {0, "Societies and Special Interest Groups" },
            {1, "International Communities" },
            {2, "Sports and Recreation" },
        };
    }
}
