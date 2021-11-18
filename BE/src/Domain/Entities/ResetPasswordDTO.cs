using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "{0} is required *")]
        [StringLength(255, ErrorMessage = "{0} should be no longer than 255 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required *")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "{0} should be between 8 and 255 characters long")]
        [RegularExpression(@"[a-zA-Z0-9]*", ErrorMessage = "{0} should contain only alphanumeric characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password and password do not match")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
