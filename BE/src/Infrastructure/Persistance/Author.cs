using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistance
{
    public class Author
    {
        public int Id { get; set; }
        [MaxLength(35), Required]
        public string FullName { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        [Required]
        public bool IsPending { get; set; }
        public virtual Notification Notification { get; set; }
    }
}

