using System.ComponentModel.DataAnnotations;

namespace ActivityFinder.Server.Models
{
    public class CommentDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Komentarz nie może być pusty")]
        public required string Content { get; set; }
    }
}
