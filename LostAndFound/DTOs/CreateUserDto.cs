using LostAndFound.Enums;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public CreateUserDto() { }
    }
}
