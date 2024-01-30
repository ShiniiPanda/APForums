using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO.PagesDTO
{
    public class SinglePost
    {

        public Post Post { get; set; } = null!;

        public List<PostTag> Tags { get; set; } = new();

        public List<PostImpression> Impressions { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();

        public string ForumName { get; set; }
    }
}
