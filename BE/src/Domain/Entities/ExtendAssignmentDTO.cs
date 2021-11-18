using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ExtendAssignmentDTO
    {
        [Required]
        public int AssignmentId { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        [RegularExpression(@"[\ -z]{5,100}$")]
        public string Reason { get; set; }
    }
}
