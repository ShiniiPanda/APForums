using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class SocialLink
    {


        public int Id { get; set; }
#nullable enable

        public string Value { get; set; } = null!;

        public int Type { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int UserId { get; set; }

#nullable disable

        public readonly static Dictionary<int, string> SocialLinkTypes = new()
        {
            {0, "Email" },
            {1, "Discord" },
            {2, "Tiktok" },
            {3, "Instagram" },
            {4, "Whatsapp" },
            {5, "Facebook" },
            {6, "Reddit" },
        };

    }
}