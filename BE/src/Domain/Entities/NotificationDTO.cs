using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class NotificationDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Message { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool AdminScope { get; set; }

        [MaxLength(100)]
        public string ActionPath { get; set; }
    }
}
