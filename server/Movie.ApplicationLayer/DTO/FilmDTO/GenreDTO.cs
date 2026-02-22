using Movie.Domain.Entities;

namespace Movie.Application.DTO.GenreDTO;

public class GenreDTO
{
    public Guid genreId {get; set;}
    public string? GenreTitle {get; set;}
}