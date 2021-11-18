using Infrastructure.Persistance;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class BookDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string ImagePath { get; set; }
        public string Owner { get; set; }
        public string Language { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public string Author { get; set; }
        public bool IsPending { get; set; }
        public bool IsAvailable { get; set; }
    }
}
