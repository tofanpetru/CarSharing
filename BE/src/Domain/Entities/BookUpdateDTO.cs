using System;

namespace Domain.Entities
{
    public class BookUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string ImagePath { get; set; }
        public string Owner { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public bool IsPending { get; set; }
    }
}
