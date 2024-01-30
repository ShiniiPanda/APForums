using APForums.Client.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Structures
{
    public class ClubGroup
    {

        public List<Club> Clubs = new();

        public int Type { get; set; }

    }
}
