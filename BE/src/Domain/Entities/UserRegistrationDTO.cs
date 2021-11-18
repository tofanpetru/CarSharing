using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserRegistrationDTO
    {
        [Required(ErrorMessage = "{0} is required *")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be between 3 and 50 characters long")]
        [RegularExpression(@"[\p{L}]*", ErrorMessage = "{0} should contain only alphabetic characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required *")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be between 3 and 50 characters long")]
        [RegularExpression(@"[\p{L}]*", ErrorMessage = "{0} should contain only alphabetic characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} is required *")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be between 3 and 50 characters long")]
        [RegularExpression(@"[a-zA-Z0-9]*", ErrorMessage = "{0} should contain only alphanumeric characters")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

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

    }
}
