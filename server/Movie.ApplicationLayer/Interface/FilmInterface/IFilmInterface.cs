using Movie.Application.DTO.FilmDTO;
using Movie.Domain.Entities;

namespace Movie.Application.Interface.FilmInterface;

public interface IFilmInterface
{
    Task<List<FilmDisplayDTO>> GetFilmsAsync();
    Task<Film> FilterByGenre(Genre genre);
    Task<Film> FilterByReleaseYear(string year);
    Task<FilmDataDTO> ShowMovieDetails(Film film);
    Task<Film> AddMovieDetails(Film film);
}