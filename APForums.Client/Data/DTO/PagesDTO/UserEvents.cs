using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO.PagesDTO
{
    public class UserEvents
    {

        public List<EventWithInterest> PublicEvents { get; set; } = new();
        
        public List<EventWithInterest> PrivateEvents { get; set; } = new();

    }
}
