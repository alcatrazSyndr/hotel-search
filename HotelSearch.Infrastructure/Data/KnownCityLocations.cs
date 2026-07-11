using HotelSearch.Domain.ValueObjects;
using HotelSearch.Infrastructure.Helpers;

namespace HotelSearch.Infrastructure.Data
{
    public static class KnownCityLocations
    {
        private static readonly Dictionary<string, GeoLocation> _cityLocationsDictionary = new(StringComparer.OrdinalIgnoreCase)
        {
            { "zagreb", new GeoLocation(45.815, 15.9819) },
            { "split", new GeoLocation(43.5081, 16.4402) },
            { "dakovo", new GeoLocation(45.3097, 18.4106) },
            { "djakovo", new GeoLocation(45.3097, 18.4106) },
            { "vienna", new GeoLocation(48.2082, 16.3738) },
            { "valletta", new GeoLocation(35.8989, 14.5146) },
            { "london", new GeoLocation(51.5074, -0.1278) },
        };

        public static GeoLocation? GetCityLocation(string cityName)
        {
            var normalizedName = TextNormalizer.RemoveDiacritics(cityName);
            var found = _cityLocationsDictionary.TryGetValue(normalizedName, out var location);

            if (!found)
            {
                return null;
            }

            return location;
        }

        public static IEnumerable<string> GetAllCityNames()
        {
            return _cityLocationsDictionary.Keys;
        }
    }
}
