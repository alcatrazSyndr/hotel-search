using HotelSearch.Application.ValueObjects;

namespace HotelSearch.Application.Interfaces
{
    public interface IHotelSearchPromptParser
    {
        Task<HotelSearchParameters> ParseSearchPromptAsync(string prompt);
    }
}
