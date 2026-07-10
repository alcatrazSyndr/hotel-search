using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public GeoLocation Location { get; set; }
    }
}
