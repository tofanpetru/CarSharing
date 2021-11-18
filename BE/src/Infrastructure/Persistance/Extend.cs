using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistance
{
    public class Extend
    {
        public int Id { get; set; }
        [MaxLength(100), Required]
        public string Reason { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public virtual Assignment Assignment { get; set; }
        public bool Approved { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
