using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence
{
    public class CarBrand
    {
        public int Id { get; set; }
        [Required, MaxLength(30), DataType(DataType.Text)]
        public string Name { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
