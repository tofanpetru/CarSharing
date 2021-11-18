using System.Collections.Generic;

namespace Infrastructure.Persistance
{
    public class WishBook
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
