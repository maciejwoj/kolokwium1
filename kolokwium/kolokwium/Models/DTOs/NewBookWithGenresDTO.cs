using System.ComponentModel.DataAnnotations;

namespace kolokwium.Models.DTOs;

public class NewBookWithGenresDTO
{
    
    [Required]
    public int PK{ get; set; }
    [Required]
    public string Title { get; set; }
    public List<int> Genres { get; set; }
}