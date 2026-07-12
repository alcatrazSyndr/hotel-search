namespace HotelSearch.Application.DTOs
{
    public class HotelSearchResultDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double? DistanceInKilometers { get; set; }
    }
}