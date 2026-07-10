using HotelSearch.Domain.Entities;

namespace HotelSearch.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel> AddAsync(Hotel hotel);
        Task<Hotel?> GetByIdAsync(int id);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel?> UpdateAsync(Hotel hotel);
        Task<bool> DeleteAsync(int id);
    }
}
