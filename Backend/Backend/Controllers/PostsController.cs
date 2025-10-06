using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Backend.Data;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> CreatePost([FromBody] Post model)
        {
            // Get logged-in user's ID from token (JWT claim)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Content))
                return BadRequest("Title and content are required.");

            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                AuthorID = int.Parse(userId), // ✅ EF model property should be AuthorId (PascalCase)
                CreatedAt = DateTime.Now
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Post created successfully.",
                postId = post.PostID,
                post.Title,
                post.Content,
                post.CreatedAt
            });
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.PostID,
                    p.Title,
                    p.Content,
                    Author = p.Author.FirstName + " " + p.Author.LastName,
                    p.CreatedAt,
                    p.UpdatedAt
                })
                .ToListAsync();

            return Ok(posts);
        }

        // --------------------------------------------------------------------
        // PUT /api/posts/{id} → Update own post
        // --------------------------------------------------------------------
        [HttpPut("{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post updated)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found.");

            if (post.AuthorID != int.Parse(userId))
                return Forbid("You can only update your own posts.");

            if (string.IsNullOrWhiteSpace(updated.Title) || string.IsNullOrWhiteSpace(updated.Content))
                return BadRequest("Title and content are required.");

            post.Title = updated.Title;
            post.Content = updated.Content;
            post.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Post updated successfully." });
        }

        // --------------------------------------------------------------------
        // DELETE /api/posts/{id} → Delete own post
        // --------------------------------------------------------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found.");

            if (post.AuthorID != int.Parse(userId))
                return Forbid("You can only delete your own posts.");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Post deleted successfully." });
        }



        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] int? authorId, [FromQuery] string? sortBy)
        {
            var postsQuery = _context.Posts
                .Include(p => p.Author)
                .AsQueryable();

            // Filter by author
            if (authorId.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.AuthorID == authorId.Value);
            }

            // Sorting
            postsQuery = sortBy?.ToLower() switch
            {
                "recent" => postsQuery.OrderByDescending(p => p.CreatedAt),
                "likes" => postsQuery
                    .OrderByDescending(p => p.Interactions.Count(pi => pi.InteractionType == "Like")),
                _ => postsQuery.OrderByDescending(p => p.CreatedAt) // default
            };

            var posts = await postsQuery
                .Select(p => new
                {
                    p.PostID,
                    p.Title,
                    p.Content,
                    Author = p.Author.Username,
                    p.CreatedAt
                })
                .ToListAsync();

            return Ok(posts);
        }
    }
}

