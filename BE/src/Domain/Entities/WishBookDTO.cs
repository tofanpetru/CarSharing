using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class WishBookDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Book Title Required")]
        [RegularExpression(@"^([\s.]*([^\s.][\s.]*){3,100})$",ErrorMessage = "Not Valid Title")]
        public string BookTitle { get; set; }
        [Required(ErrorMessage ="Book Author Required")]
        [RegularExpression("^[a-zA-Z]{2,15}(?: [a-zA-Z]{2,15})$",ErrorMessage = "Not Valid Full Name")]
        public string BookAuthor { get; set; }
        public virtual ICollection<string> Users { get; set; }
    }
}
