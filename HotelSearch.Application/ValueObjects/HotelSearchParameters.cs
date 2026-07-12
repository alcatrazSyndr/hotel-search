using HotelSearch.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace HotelSearch.Application.ValueObjects
{
    public readonly struct HotelSearchParameters
    {
        public GeoLocation? GeoLocation { get; }
        public decimal? MinPrice { get; }
        public decimal? MaxPrice { get; }

        [JsonConstructor]
        public HotelSearchParameters(decimal? minPrice, decimal? maxPrice, GeoLocation? geoLocation)
        {
            if (minPrice.HasValue && minPrice.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minPrice), "Minimum price cannot be negative.");
            }

            if (maxPrice.HasValue && maxPrice.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxPrice), "Maximum price cannot be negative.");
            }

            if (minPrice.HasValue && maxPrice.HasValue && minPrice.Value > maxPrice.Value)
            {
                throw new ArgumentException("Minimum price cannot be greater than maximum price.");
            }

            MinPrice = minPrice;
            MaxPrice = maxPrice;
            GeoLocation = geoLocation;
        }
    }
}