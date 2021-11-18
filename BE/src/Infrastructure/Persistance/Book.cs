using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistance
{
    public class Book
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        [MaxLength(50)]
        public string ImagePath { get; set; }
        public virtual User Owner { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual Author Author { get; set; }
        [Required]
        public bool IsPending { get; set; }
        public bool IsAvailable { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
    }
}
