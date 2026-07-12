using HotelSearch.Application.DTOs;
using HotelSearch.Application.Interfaces;

namespace HotelSearch.Application.Services
{
    public class HotelSearchService : IHotelSearchService
    {
        private const double PriceWeight = 1.0;
        private const double DistanceWeight = 3.0;

        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelSearchPromptParser _promptParser;

        public HotelSearchService(IHotelRepository hotelRepository, IHotelSearchPromptParser promptParser)
        {
            _hotelRepository = hotelRepository;
            _promptParser = promptParser;
        }

        public async Task<PagedResultDTO<HotelSearchResultDTO>> SearchAsync(string prompt, int page, int pageSize)
        {
            var hotelSearchParameters = await _promptParser.ParseSearchPromptAsync(prompt);
            var allHotels = await _hotelRepository.GetAllAsync();

            // Filter hotels by price
            var filteredHotels = allHotels
                .Where(hotel =>
                    (!hotelSearchParameters.MinPrice.HasValue || hotel.Price >= hotelSearchParameters.MinPrice.Value) &&
                    (!hotelSearchParameters.MaxPrice.HasValue || hotel.Price <= hotelSearchParameters.MaxPrice.Value))
                .ToList();

            var hotelSearchResultDTOList = new List<HotelSearchResultDTO>();
            var result = new PagedResultDTO<HotelSearchResultDTO>()
            {
                Page = page,
                PageSize = pageSize,
                TotalResultCount = 0,
                TotalPages = 0,
                PageResultItems = hotelSearchResultDTOList
            };

            if (filteredHotels.Count == 0)
            {
                return result;
            }

            // Using a tuple here instead of a dedicated class since this pairing is only
            // used locally within this method, with no reuse or standalone identity elsewhere.
            var hotelsWithScoreAndDistance = filteredHotels
                .Select(hotel =>
                    (Hotel: hotel, Score: 0.0, Distance: hotelSearchParameters.GeoLocation.HasValue
                        ? hotel.Location.DistanceToInKilometers(hotelSearchParameters.GeoLocation.Value)
                        : (double?)null))
                .ToList();

            // Using a manual single-pass loop here instead of separate LINQ .Min()/.Max() calls
            // to avoid iterating hotelsWithDistance multiple times
            decimal? minPrice = null;
            decimal? maxPrice = null;
            double? minDistance = null;
            double? maxDistance = null;
            foreach (var t in hotelsWithScoreAndDistance)
            {
                if (!minPrice.HasValue || t.Hotel.Price < minPrice)
                {
                    minPrice = t.Hotel.Price;
                }
                if (!maxPrice.HasValue || t.Hotel.Price > maxPrice)
                {
                    maxPrice = t.Hotel.Price;
                }
                if (t.Distance.HasValue)
                {
                    if (!minDistance.HasValue || t.Distance < minDistance)
                    {
                        minDistance = t.Distance.Value;
                    }
                    if (!maxDistance.HasValue || t.Distance > maxDistance)
                    {
                        maxDistance = t.Distance.Value;
                    }
                }
            }

            for (int i = 0; i < hotelsWithScoreAndDistance.Count; i++)
            {
                var t = hotelsWithScoreAndDistance[i];
                var score = 0.0;
                var price = t.Hotel.Price;

                if (maxPrice > minPrice)
                {
                    var normalizedPrice = (double)((price - minPrice) / (maxPrice - minPrice));
                    score += normalizedPrice * PriceWeight;
                }

                if (t.Distance.HasValue && minDistance.HasValue && maxDistance.HasValue && maxDistance > minDistance)
                {
                    var distance = t.Distance.Value;
                    var normalizedDistance = (distance - minDistance.Value) / (maxDistance.Value - minDistance.Value);
                    score += normalizedDistance * DistanceWeight;
                }

                hotelsWithScoreAndDistance[i] = (t.Hotel, score, t.Distance);
            }

            hotelsWithScoreAndDistance.Sort((x1, x2) => x1.Score.CompareTo(x2.Score));

            result.TotalResultCount = hotelsWithScoreAndDistance.Count;
            result.TotalPages = (int)Math.Ceiling(hotelsWithScoreAndDistance.Count / (double)pageSize);

            hotelSearchResultDTOList = hotelsWithScoreAndDistance
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => 
                    new HotelSearchResultDTO
                    {
                        Name = t.Hotel.Name,
                        Price = t.Hotel.Price,
                        DistanceInKilometers = t.Distance
                    })
                .ToList();

            result.PageResultItems = hotelSearchResultDTOList;

            return result;
        }
    }
}