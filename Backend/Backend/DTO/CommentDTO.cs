namespace Backend.DTO
{
    public class CommentDTO
    {
        public int CommentID { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Author details
        public int AuthorID { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}
