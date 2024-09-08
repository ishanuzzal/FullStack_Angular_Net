using System.ComponentModel.DataAnnotations;

namespace MyProject.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class ConfirmUserDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
