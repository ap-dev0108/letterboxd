using Movie.Application.DTO.GenreDTO;
using Movie.Domain.Entities;

namespace Movie.Application.Interface.FilmInterface.GenreInterface;

public interface IGenreInterface
{
    Task<GenreDTO> AddGenre(GenreDTO genre);
    Task<GenreDTO> UpdateGenreById(Guid id, GenreDTO genreDTO);
    Task<GenreDTO> DeleteGenreById(GenreDTO genre);
}