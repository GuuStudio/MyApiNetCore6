using System.ComponentModel.DataAnnotations;

namespace MyApiNetCore6.Models
{
    public class SignUpModel
    {
        [Required]
        public string FirstName { set; get; } = null!;
        [Required]
        public string LastName { set; get; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;

    }
}
