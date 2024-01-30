using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO.PagesDTO
{
    public class SingleClub
    {
        public Club Club { get; set; } = null!;

        public List<Forum> Forums { get; set; } = null!;

        public List<EventWithInterest> Events { get; set; } = null!;

        public int MemberCount { get; set; } = 0;

        public int Role { get; set; } = 0;
    }
}
