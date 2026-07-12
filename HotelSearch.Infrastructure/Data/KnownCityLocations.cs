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
    { "rijeka", new GeoLocation(45.3271, 14.4422) },
    { "dubrovnik", new GeoLocation(42.6507, 18.0944) },
    { "osijek", new GeoLocation(45.5550, 18.6955) },
    { "zadar", new GeoLocation(44.1194, 15.2314) },
    { "pula", new GeoLocation(44.8666, 13.8496) },
    { "vienna", new GeoLocation(48.2082, 16.3738) },
    { "valletta", new GeoLocation(35.8989, 14.5146) },
    { "london", new GeoLocation(51.5074, -0.1278) },
    { "paris", new GeoLocation(48.8566, 2.3522) },
    { "berlin", new GeoLocation(52.5200, 13.4050) },
    { "rome", new GeoLocation(41.9028, 12.4964) },
    { "madrid", new GeoLocation(40.4168, -3.7038) },
    { "amsterdam", new GeoLocation(52.3676, 4.9041) },
    { "lisbon", new GeoLocation(38.7223, -9.1393) },
    { "prague", new GeoLocation(50.0755, 14.4378) },
    { "budapest", new GeoLocation(47.4979, 19.0402) },
    { "warsaw", new GeoLocation(52.2297, 21.0122) },
    { "athens", new GeoLocation(37.9838, 23.7275) },
    { "dublin", new GeoLocation(53.3498, -6.2603) },
    { "brussels", new GeoLocation(50.8503, 4.3517) },
    { "copenhagen", new GeoLocation(55.6761, 12.5683) },
    { "stockholm", new GeoLocation(59.3293, 18.0686) },
    { "oslo", new GeoLocation(59.9139, 10.7522) },
    { "helsinki", new GeoLocation(60.1699, 24.9384) },
    { "zurich", new GeoLocation(47.3769, 8.5417) },
    { "munich", new GeoLocation(48.1351, 11.5820) },
    { "milan", new GeoLocation(45.4642, 9.1900) },
    { "barcelona", new GeoLocation(41.3874, 2.1686) },
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
