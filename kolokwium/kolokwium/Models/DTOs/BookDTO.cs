namespace kolokwium.Models.DTOs;

public class BookDTO
{
    public int PK { get; set; }
    public string title { get; set; } = string.Empty;
    public List<GenreDTO> GenreDtos { get; set; } = null!;
}

public class GenreDTO
{
    public int PK { get; set; }
    public string nam { get; set; } = string.Empty;
    
}