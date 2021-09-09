using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MaxLength(50), DataType(DataType.Text)]
        public string Title { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
