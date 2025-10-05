using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class UserCreateDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Admin|Employee)$", ErrorMessage = "Role must be 'Admin' or 'Employee'.")]
        public string Role { get; set; } = "Employee";
    }
}
