using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ResponsePendingRequestDTO
    {
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z]{2,15}(?: [a-zA-Z]{2,15})$")]
        public string Author { get; set; }
    }
}
