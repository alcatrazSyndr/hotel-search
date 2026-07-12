namespace HotelSearch.Application.DTOs
{
    public class PagedResultDTO<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalResultCount { get; set; }
        public IEnumerable<T> PageResultItems { get; set; } = Enumerable.Empty<T>();
    }
}