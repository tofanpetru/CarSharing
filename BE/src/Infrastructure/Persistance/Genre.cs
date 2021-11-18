using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistance
{
    public class Genre
    {
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
