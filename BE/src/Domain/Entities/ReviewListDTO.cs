using System;

namespace Domain.Entities
{
    public class ReviewListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string BookTitle { get; set; }
        public string ReviewerUserName { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
