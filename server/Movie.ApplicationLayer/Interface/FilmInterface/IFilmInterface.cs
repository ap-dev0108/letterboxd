using Movie.Application.DTO.AddFilm;
using Movie.Application.DTO.FilmDTO;
using Movie.Application.DTO.UpdateFilm;
using Movie.Domain.Entities;

namespace Movie.Application.Interface.FilmInterface;

public interface IFilmInterface
{
    Task<List<AddFilmDTO>> GetFilmsAsync();
    Task<Film> FilterByGenre(Genre genre);
    Task<Film> FilterByReleaseYear(string year);
    Task<Film> ShowMovieDetails(Guid movieId);
    Task<Film> AddMovieDetails(AddFilmDTO filmDTO);
    Task<int> GetTotalRatings(Guid movieId);
    Task<Film> UpdateFilm(Guid movieid, UpdateFilmDTO updateFilmDTO);
}