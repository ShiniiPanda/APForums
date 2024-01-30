using APForums.Server.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace APForums.Server.Data.DTO
{
    public class EventUploadDTO
    {

        public Event Event { get; set; } = null!;

        public IFormFile? File { get; set; }

    }
}
