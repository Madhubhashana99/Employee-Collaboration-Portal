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
        // private readonly IPasswordHasher _passwordHasher; 

        public UserService(ApplicationDbContext context /*, IPasswordHasher passwordHasher*/)
        {
            _context = context;
            // _passwordHasher = passwordHasher;
        }

        public async Task<UserResponseDTO> CreateUserAsync(UserCreateDTO createDto)
        {
            // 1. Check for duplicate username (business rule)
            if (await _context.Users.AnyAsync(u => u.Username == createDto.Username))
            {
                throw new ArgumentException("Username is already taken.");
            }

            // 2. Map DTO to User Entity
            var user = new User
            {
                Username = createDto.Username,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Role = createDto.Role,
                // 3. HASH THE PASSWORD! Never store plain text passwords.
                PasswordHash = HashPassword(createDto.Password) // Use a proper hashing function
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 4. Return the new user as the response DTO
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
            // Fetch all users and map to response DTOs
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
