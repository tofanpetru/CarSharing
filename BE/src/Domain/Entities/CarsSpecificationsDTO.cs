namespace Domain.Entities
{
    public class CarsSpecificationsDTO
    {
        public int Seats { get; set; }
        public decimal PricePerDay { get; set; }
        public string Fuel { get; set; }
        public string Color { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public bool IsAvalable { get; set; }
        public int Kilometers { get; set; }
    }
}
