using System;

namespace Domain.Entities
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string OwnerId { get; set; }
        public bool IsCurrentOwner { get; set; }
    }
}
