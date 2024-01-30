using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class Forum
    {

#nullable enable
        public int? Id { get; set; }

        public string? Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? Visibility { get; set; }

        public string? Intake { get; set; }

        public int? ClubId { get; set; }

        public string? ClubName { get; set; }
#nullable disable

        public string GetDescriptionPreview()
        {
            if (Description != null)
            {
                if (Description.Length > DescriptionPreviewLength)
                {
                    return $"{Description.Substring(0, DescriptionPreviewLength - 1)}..";
                }
                else
                {
                    return Description;
                }
            }
            else
            {
                return "";
            }
        }

        public static int DescriptionPreviewLength = 80;

    }
}
