using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data
{ 
    public class User
    {
        public int Id { get; set; }

#nullable enable

        public string? TPNumber { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Picture { get; set; }

        public string? DOB { get; set; }

        public string? DegreeType { get; set; }

        public string? Department { get; set; }

        public string? Course { get; set; } = null;

        public string? Enrollment { get; set; }

        public int? Level { get; set; }

        public string? Intake { get; set; }

#nullable disable

        public static User GetDefaultUserInfo()
        {
            return new User
            {
                Id = 5,
                TPNumber = "TP022321"
            };
        }

        public static List<string> Pictures { get; } = new()
        {
            "default_1.png",
            "default_2.png",
            "default_3.png",
            "default_4.png",
            "default_5.png",
            "default_6.png"
        };
    }
}
