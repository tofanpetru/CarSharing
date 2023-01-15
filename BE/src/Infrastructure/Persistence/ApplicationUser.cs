using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence
{
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.ImageUrl)]
        public string ProfileImage { get; set; }
    }
}
