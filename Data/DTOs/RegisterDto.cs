using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password length must be between 6 and 100 characters", MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }
}
