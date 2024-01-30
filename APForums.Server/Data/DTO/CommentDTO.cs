using APForums.Server.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;

namespace APForums.Server.Data.DTO
{
    public class CommentDTO
    {
        [JsonConstructor]
        public CommentDTO()
        {

        }

        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            Content = comment.Content;
            PostedDate = comment.PostedDate;
            LastUpdated = comment.LastUpdated;
            Visible = comment.Visible;
            PostId = comment.PostId;
            User.Id = comment.UserId;
            if (comment.Impressions.Count() > 0)
            {
                Impressions = comment.Impressions.Select(x => new CommentImpressionDTO(x)).ToList();
            }
        }

        public int Id { get; set; }

        public string? Content { get; set; } = null!;

        public DateTime? PostedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public bool Visible { get; set; } = true;

        public int PostId { get; set; }

        public BasicUserDTO User { get; set; } = new();

        public ICollection<CommentImpressionDTO> Impressions { get; } = new List<CommentImpressionDTO>();

    }
}
