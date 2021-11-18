using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistance
{
    public class User : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string ProfileImagePath { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<WishBook> WishBooks { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
