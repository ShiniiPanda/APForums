using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Structures
{
    public class ImpressionGroup<T>
    {
        public int Value { get; set; }

        public List<T> Impressions { get; set; }
    }
}
