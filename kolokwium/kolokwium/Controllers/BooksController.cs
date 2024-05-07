using kolokwium.Models.DTOs;
using kolokwium.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium.Controllers


{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BooksController(IBooksRepository animalsRepository)
        {
            _booksRepository = animalsRepository;
        }

        [HttpGet("{id}/genres")]
        public async Task<IActionResult> GetGenresBook(int id)
        {
            if (!await _booksRepository.DoesBookExist(id))
                return NotFound($"book with given ID - {id} doesn't exist");

            var genres = await _booksRepository.GetGenresBook(id);

            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(NewBookWithGenresDTO newBookWithGenresDto)
        {
            await _booksRepository.AddBookWithGenres(newBookWithGenresDto);

            return Created(Request.Path.Value ?? "api/books", newBookWithGenresDto);
        }
    }
    
    
}
        
        
