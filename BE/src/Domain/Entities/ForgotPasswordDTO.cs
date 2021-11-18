using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "{0} is required *")]
        [StringLength(255, ErrorMessage = "{0} should be no longer than 255 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
