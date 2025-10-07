using Backend.Data;
using Backend.DTO;
using Backend.Models;
using Backend.Services;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDTO> CreateUserAsync(UserCreateDTO createDto)
        {
            
            if (await _context.Users.AnyAsync(u => u.Username == createDto.Username))
            {
                throw new ArgumentException("Username is already taken.");
            }

            
            var user = new User
            {
                Username = createDto.Username,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Role = createDto.Role,
                PasswordHash = HashPassword(createDto.Password) 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        
            return new UserResponseDTO
            {
                UserId = user.UserID,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            
            return await _context.Users
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserID,
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role
                })
                .ToListAsync();
        }

        private string HashPassword(string password) => password;
    }
}
