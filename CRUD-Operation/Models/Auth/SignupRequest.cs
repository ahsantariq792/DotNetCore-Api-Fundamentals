using System.ComponentModel.DataAnnotations;

namespace CRUD_Operation.Models.Auth
{
    public class SignupRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public required string Password { get; set; }

        public string? FullName { get; set; }
    }
}
