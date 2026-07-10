namespace HotelSearch.Domain.ValueObjects
{
    public readonly struct GeoLocation
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public GeoLocation(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        public double DistanceToInKilometers(GeoLocation other)
        {
            const double earthRadiusInKilometers = 6371.0;

            var latitudeDifferenceInRadians = DegreesToRadians(other.Latitude - Latitude);
            var longitudeDifferenceInRadians = DegreesToRadians(other.Longitude - Longitude);

            var a = Math.Sin(latitudeDifferenceInRadians / 2) * Math.Sin(latitudeDifferenceInRadians / 2) +
                    Math.Cos(DegreesToRadians(Latitude)) * Math.Cos(DegreesToRadians(other.Latitude)) *
                    Math.Sin(longitudeDifferenceInRadians / 2) * Math.Sin(longitudeDifferenceInRadians / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadiusInKilometers * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
