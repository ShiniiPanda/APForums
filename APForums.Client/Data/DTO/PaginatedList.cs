using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.DTO
{
    //Holds server-side pagination resposnes.
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public int CurrentPage { get; set; }

        public int PageSize { get; set; } 

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

    }
}
