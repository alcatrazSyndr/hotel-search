using HotelSearch.Application.DTOs;

namespace HotelSearch.Application.Interfaces
{
    public interface IHotelSearchService
    {
        Task<PagedResultDTO<HotelSearchResultDTO>> SearchAsync(string prompt, int page, int pageSize);
    }
}
