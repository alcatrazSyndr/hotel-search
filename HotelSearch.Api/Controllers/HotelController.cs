using HotelSearch.Application.DTOs;
using HotelSearch.Application.Interfaces;
using HotelSearch.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelSearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelSearchService _hotelSearchService;

        public HotelController(IHotelRepository hotelRepository, IHotelSearchService hotelSearchService)
        {
            _hotelRepository = hotelRepository;
            _hotelSearchService = hotelSearchService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetAll()
        {
            var hotels = await _hotelRepository.GetAllAsync();

            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetById(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> Create([FromBody] Hotel hotel)
        {
            var addedHotel = await _hotelRepository.AddAsync(hotel);

            return CreatedAtAction(nameof(GetById), new { id = addedHotel.Id }, addedHotel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Hotel>> Update(int id, [FromBody] Hotel hotel)
        {
            hotel.Id = id;
            var updatedHotel = await _hotelRepository.UpdateAsync(hotel);

            if (updatedHotel == null)
            {
                return NotFound();
            }

            return Ok(updatedHotel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removed = await _hotelRepository.DeleteAsync(id);

            if (!removed)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResultDTO<HotelSearchResultDTO>>> Search([FromQuery] string search = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1)
            {
                return BadRequest("Page must be 1 or greater.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page size must be between 1 and 100.");
            }

            var result = await _hotelSearchService.SearchAsync(search, page, pageSize);

            return Ok(result);
        }
    }
}
