using Domain.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ResponseEditBookDTO
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        [RegularExpression(@"[\s.]*([^\s.][\s.]*){3,100}$")]
        public string Title { get; set; }
        [Required]
        [Date]
        public DateTime PublishDate { get; set; }
        [Required]
        public virtual string Language { get; set; }
        [Required]
        public virtual ICollection<string> Genres { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z]{2,15}(?: [a-zA-Z]{2,15})$")]
        public virtual string Author { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
