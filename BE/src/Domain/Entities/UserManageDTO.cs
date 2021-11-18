using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserManageDTO
    {
        [Required]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}