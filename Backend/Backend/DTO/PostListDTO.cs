namespace Backend.DTO
{
    public class PostListDTO
    {
        public int PostID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ContentSnippet { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Author details
        public int AuthorID { get; set; }
        public string AuthorName { get; set; } = string.Empty;

        // Aggregated Interaction Data
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public int CommentCount { get; set; }

        // Optional: for 'trending' posts
        public bool IsTrending => TotalLikes > 5;
    }
}
