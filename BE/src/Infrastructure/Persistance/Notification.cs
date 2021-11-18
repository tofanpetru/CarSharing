using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistance
{
    public class Notification
    {
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Message { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool AdminScope { get; set; }
        public virtual ICollection<User> Users { get; set; }

        [MaxLength(100)]
        public string ActionPath { get; set; }
        public virtual Book Book { get; set; }
        public virtual Review Review { get; set; }
        public virtual Author PendingAuthor { get; set; }
        public virtual Extend PendingExtend { get; set; }
        public int? ReviewId { get; set; }
        public int? ExtendId { get; set; }
        public int? AuthorId { get; set; }
    }
}
