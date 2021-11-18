using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "{0} is required *")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required *")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
