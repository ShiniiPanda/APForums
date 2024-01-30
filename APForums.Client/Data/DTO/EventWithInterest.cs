using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class EventWithInterest
    {

        public Event Event { get; set; }

        public bool IsInterested { get; set; } = false;

    }
}
