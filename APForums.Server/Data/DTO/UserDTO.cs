using APForums.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Server.Data.DTO
{
    public class UserDTO
    {

        public UserDTO()
        {

        }

        public UserDTO(User user)
        {
            Id = user.Id;
            TPNumber = user.TPNumber;
            Name = user.Name;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Picture = user.Picture;
            DOB = user.DOB.ToString();
            DegreeType = user.DegreeType.ToString();
            Department = user.Department.ToString();
            Course = user.Course.ToString();
            Enrollment = user.Enrollment.ToString();
            Level = user.Level;
            Intake = user.IntakeCode;
        }

        public int? Id { get; set; }

        public string? TPNumber { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Picture { get; set; }

        public string? DOB { get; set; }

        public string? DegreeType { get; set; }

        public string? Department { get; set; }

        public string? Course { get; set; }

        public string? Enrollment { get; set; }

        public int? Level { get; set; }

        public string? Intake { get; set; }
    }
}
