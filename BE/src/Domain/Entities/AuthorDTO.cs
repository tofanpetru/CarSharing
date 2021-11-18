using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AuthorDTO
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Name")]
        public string FullName { get; set; }
    }
}
