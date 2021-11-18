using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AddReviewDTO
    {
        [Required]
        [RegularExpression(@"[\s]*([^\s][\s]*){5,100}$")]
        public string Title { get; set; }

        [RegularExpression(@"^([\s]*([^\s][\s]*){10,500})?$")]
        public string Content { get; set; }

        [Required, Range(1,5)]
        public int Rating { get; set; }
        public int BookId { get; set; }
    }
}