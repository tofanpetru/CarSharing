using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PendingRequestsDTO
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Author { get; set; }
    }
}
