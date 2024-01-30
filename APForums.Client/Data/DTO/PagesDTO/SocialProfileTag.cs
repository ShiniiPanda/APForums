using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO.PagesDTO
{
    public class SocialProfileTag
    {

        public ProfileTag Tag { get; set; }

        public List<UserConnection> Users { get; set; }


        public bool Subscribed = false;

    }
}
