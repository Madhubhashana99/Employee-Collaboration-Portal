namespace Backend.DTO
{
    public class PostInteractionDto
    {
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string InteractionType { get; set; } = string.Empty;
    }
}
