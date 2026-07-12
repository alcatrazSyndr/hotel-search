using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Tests
{
    public class GeoLocationTests
    {
        [Fact]
        public void DistanceToInKilometers_ReturnsZero_WhenSameLocation()
        {
            // Arrange
            var location = new GeoLocation(45.815, 15.9819);

            // Act
            var distance = location.DistanceToInKilometers(location);

            // Assert
            Assert.Equal(0, distance, precision: 5);
        }

        [Fact]
        public void DistanceToInKilometers_ReturnsExpectedDistance_BetweenTwoKnownCities()
        {
            // Arrange
            // Zagreb and Vienna, real-world distance is approximately 270 km
            var zagreb = new GeoLocation(45.815, 15.9819);
            var vienna = new GeoLocation(48.2082, 16.3738);
            var approximateDistance = 270;
            var margin = 10;

            // Act
            var distance = zagreb.DistanceToInKilometers(vienna);

            // Assert
            Assert.InRange(distance, approximateDistance - margin, approximateDistance + margin);
        }

        [Fact]
        public void Constructor_Throws_WhenLatitudeOutOfRange()
        {
            // Arrange
            var latitude = 999.0;
            var longitude = 15.9819;

            // Act
            var exception = Record.Exception(() => new GeoLocation(latitude, longitude));

            // Assert
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void Constructor_Throws_WhenLongitudeOutOfRange()
        {
            // Arrange
            var latitude = 45.815;
            var longitude = 999.0;

            // Act
            var exception = Record.Exception(() => new GeoLocation(latitude, longitude));

            // Assert
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void Constructor_Succeeds_WithValidCoordinates()
        {
            // Arrange
            var latitude = 45.815;
            var longitude = 15.9819;

            // Act
            var location = new GeoLocation(latitude, longitude);

            // Assert
            Assert.Equal(latitude, location.Latitude);
            Assert.Equal(longitude, location.Longitude);
        }
    }
}
