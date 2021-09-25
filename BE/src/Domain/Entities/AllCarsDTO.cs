using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class AllCarsDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]

        public string Title { get; set; }
        public virtual CarBrand CarBrand { get; set; }
        public DateTime PublishDate { get; set; }
        [DataType(DataType.ImageUrl)]
        public string CarImage { get; set; }
        public int Seats { get; set; }
        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal PricePerDay { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ManufacturingYear { get; set; }
        [DataType(DataType.Text)]
        public string Fuel { get; set; }
        [DataType(DataType.Text)]
        public string Color { get; set; }
        [DataType(DataType.Text)]
        public string Engine { get; set; }
        [DataType(DataType.Text)]
        public string Transmission { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        [Required]
        public bool IsAvalable { get; set; }
        public int Kilometers { get; set; }
    }
}
