using Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ExtendAssignmentRqDTO
    {
        public int Id { get; set; }
        public DateTime EndDate { get; set; }
        public string BookTitle { get; set; }
        public string AssigneeName { get; set; }
        public string Reason { get; set; }
    }
}
