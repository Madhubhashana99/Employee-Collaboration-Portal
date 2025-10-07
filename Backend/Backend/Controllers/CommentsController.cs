using Backend.Data;
using Backend.DTO;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;



namespace Backend.Controllers
{
    [Route("api/posts/{postId}/comments")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetComments(int postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostID == postId)
                .Include(c => c.Author)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentDTO
                {
                    CommentID = c.CommentID,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    AuthorID = c.AuthorID,
                    AuthorName = $"{c.Author.FirstName} {c.Author.LastName}"
                })
                .ToListAsync();

            return Ok(comments);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, [FromBody] CommentCreateDTO commentCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var comment = new Comment
            {
                PostID = postId,
                AuthorID = userId,
                Content = commentCreateDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            
            var createdComment = await _context.Comments
                .Include(c => c.Author)
                .Where(c => c.CommentID == comment.CommentID)
                .Select(c => new CommentDTO
                {
                    CommentID = c.CommentID,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    AuthorID = c.AuthorID,
                    AuthorName = $"{c.Author.FirstName} {c.Author.LastName}"
                })
                .FirstOrDefaultAsync();

            return Ok(createdComment);
        }
    }
}
