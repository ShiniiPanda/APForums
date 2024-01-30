using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO.PagesDTO
{
    public class HomePage
    {

        public List<Post> Posts { get; set; } = new();

        public List<Activity> Activities { get; set; } = new();

    }
}
