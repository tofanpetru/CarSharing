using System;

namespace Domain.Entities
{
    public class HomePageCarsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string CarImage { get; set; }
        public int Seats { get; set; }
        public decimal PricePerDay { get; set; }
        public string Color { get; set; }
        public string Transmission { get; set; }
        public int Kilometers { get; set; }
    }
}
