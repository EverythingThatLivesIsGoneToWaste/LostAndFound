using System.ComponentModel.DataAnnotations;

namespace LostAndFound.DTOs
{
    public class LoginModel
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
