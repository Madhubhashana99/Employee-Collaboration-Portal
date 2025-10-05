using Backend.DTO;

namespace Backend.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> CreateUserAsync(UserCreateDTO createDto);
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();

    }
}
