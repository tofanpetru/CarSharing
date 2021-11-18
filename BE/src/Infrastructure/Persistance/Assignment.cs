using System;

namespace Infrastructure.Persistance
{
    public class Assignment
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Book Book { get; set; }
        public virtual User Assignee { get; set; }
        public bool IsActive { get; set; }
        public virtual Extend Extend { get; set; }
        public int? ExtendId { get; set; }
    }
}
