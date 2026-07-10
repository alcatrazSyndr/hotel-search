using HotelSearch.Application.Interfaces;
using HotelSearch.Domain.Entities;
using System.Collections.Concurrent;

namespace HotelSearch.Infrastructure.Repositories
{
    /// <summary>
    /// In-memory implementation of IHotelRepository, backed by a thread-safe dictionary.
    /// Intended as a stand-in until a persistent storage implementation (e.g. a database) is added.
    /// </summary>
    public class InMemoryHotelRepository : IHotelRepository
    {
        private readonly ConcurrentDictionary<int, Hotel> _hotels = new ConcurrentDictionary<int, Hotel>();
        private int _nextId = 0;

        public Task<Hotel> AddAsync(Hotel hotel)
        {
            // Guarantee a unique, thread-safe id for concurrent requests
            hotel.Id = Interlocked.Increment(ref _nextId);

            _hotels[hotel.Id] = hotel;

            return Task.FromResult(hotel);
        }

        public Task<Hotel?> GetByIdAsync(int id)
        {
            _hotels.TryGetValue(id, out var hotel);

            return Task.FromResult(hotel);
        }

        public Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Hotel>>(_hotels.Values.ToList());
        }

        public Task<Hotel?> UpdateAsync(Hotel hotel)
        {
            _hotels.TryGetValue(hotel.Id, out var existingHotel);
            if (existingHotel != null)
            {
                _hotels[hotel.Id] = hotel;

                return Task.FromResult<Hotel?>(hotel);
            }

            return Task.FromResult<Hotel?>(null);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var removed = _hotels.TryRemove(id, out var _);

            return Task.FromResult(removed);
        }
    }
}
