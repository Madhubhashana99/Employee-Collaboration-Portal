using Backend.DTO;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

     
        [HttpPost]
        [Authorize(Roles = "Admin")] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        public async Task<ActionResult<UserResponseDTO>> CreateUser([FromBody] UserCreateDTO userCreateDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = await _userService.CreateUserAsync(userCreateDto);
           
                return CreatedAtAction(nameof(ListUsers), new { id = newUser.UserId }, newUser);
            }
            catch (ArgumentException ex)
            {
                
                return BadRequest(new { Message = ex.Message });
            }
        }

       
        [HttpGet]
        [Authorize(Roles = "Admin")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> ListUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

    }
}
