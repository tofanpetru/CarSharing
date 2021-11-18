using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Infrastructure.Persistance
{
    public class Role : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
