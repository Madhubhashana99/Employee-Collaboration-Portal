using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostInteractionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostInteractionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("react")]
        public async Task<IActionResult> React([FromBody] PostInteraction dto)
        {
          
            var existing = await _context.PostInteractions
                .FirstOrDefaultAsync(p => p.PostID == dto.PostID && p.UserID == dto.UserID);

            if (existing != null)
            {
                existing.InteractionType = dto.InteractionType;
                existing.InteractionDate = DateTime.UtcNow;
            }
            else
            {
                var interaction = new PostInteraction
                {
                    PostID = dto.PostID,
                    UserID = dto.UserID,
                    InteractionType = dto.InteractionType
                };
                _context.PostInteractions.Add(interaction);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

       
        [HttpGet("{postId}/summary")]
        public async Task<IActionResult> GetSummary(int postId)
        {
            var likes = await _context.PostInteractions
                .CountAsync(p => p.PostID == postId && p.InteractionType == "Like");

            var dislikes = await _context.PostInteractions
                .CountAsync(p => p.PostID == postId && p.InteractionType == "Dislike");

            return Ok(new { PostID = postId, Likes = likes, Dislikes = dislikes });
        }
    }
}
