namespace Backend.Services
{
    public interface IAuthService
    {
        Task<string?> AuthenticateUser(string username, string password);
    }
}
