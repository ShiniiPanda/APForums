using Microsoft.Identity.Client;

namespace APForums.Server.Data.DTO
{
    public class PaginatedListDTO<T>
    {

        public List<T> Items { get; set; } = new List<T>();

        public int CurrentPage { get; set; }

        public int PageSize { get; set; } = 5;

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

    }
}
