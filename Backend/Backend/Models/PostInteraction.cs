using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class PostInteraction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InteractionID { get; set; }

        [Required]
        public int PostID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [MaxLength(8)]
        // InteractionType is enforced to be 'Like' or 'Dislike' by the DB constraint
        public string InteractionType { get; set; } = string.Empty;

        public DateTime InteractionDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey(nameof(PostID))]
        public Post Post { get; set; } = null!;

        [ForeignKey(nameof(UserID))]
        public User User { get; set; } = null!;
    }
}
