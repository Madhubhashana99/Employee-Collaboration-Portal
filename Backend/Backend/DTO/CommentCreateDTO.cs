using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class CommentCreateDTO
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; } = string.Empty;
    }
}
