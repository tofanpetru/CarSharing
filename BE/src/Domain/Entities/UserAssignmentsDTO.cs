using Domain.Attributes;
using System;

namespace Domain.Entities
{
    public class UserAssignmentsDTO
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        [Date]
        public DateTime StartDate { get; set; }
        [Date]
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsExtended { get; set; }
        public bool CanBeExtended { get; set; }
    }
}
