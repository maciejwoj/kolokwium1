using kolokwium.Models.DTOs;

namespace kolokwium.Repositories;

public interface IBooksRepository
{
    Task<bool> DoesBookExist(int id);
    Task<List<string>> GetGenresBook(int id);

    Task AddBookWithGenres(NewBookWithGenresDTO newBookWithGenresDto);
}