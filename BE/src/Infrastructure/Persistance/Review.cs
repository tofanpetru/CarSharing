using System;

namespace Infrastructure.Persistance
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int Rating { get; set; }
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
