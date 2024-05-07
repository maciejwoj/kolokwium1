namespace kolokwium.Models.DTOs;

public class NewBookWithGenresDTO
{
    public string Title { get; set; }
    public List<int> Genres { get; set; }
}