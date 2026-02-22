using Movie.Application.DTO.GenreDTO;
using Movie.Domain.Entities;

namespace Movie.Application.Interface.RepoInterface.GenreRepo;

public interface IGenreRepo
{
    Task<List<Genre>> GetAllGenre();
    Task<Genre> FindGenreByTitle(string? title);
    Task<Genre> GetGenreById(Guid id);
    Task<GenreDTO> AddGenre(GenreDTO genreDTO);
    Task UpdateGenre(Genre genre);
    Task DeleteGenre(GenreDTO genreDTO);
}