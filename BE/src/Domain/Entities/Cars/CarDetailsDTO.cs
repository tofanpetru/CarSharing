using Infrastructure.Persistence;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class CarDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CarBrand { get; set; }
        public DateTime PublishDate { get; set; }
        public string CarImage { get; set; }
        public int Seats { get; set; }
        public decimal PricePerDay { get; set; }
        public DateTime ManufacturingYear { get; set; }
        public string Fuel { get; set; }
        public string Color { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        //public ICollection<CarCategoryDTO> Categories { get; set; }
        public bool IsAvalable { get; set; }
        public int Kilometers { get; set; }
    }
}
