using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    public class ForumWithRecentPost
    {

#nullable enable
        public Forum Forum { get; set; } = null!;

        public Post? Post { get; set; }
#nullable disable
    }
}
