using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Application.DTO.AddFilm;
using Movie.Application.DTO.FilmDTO;
using Movie.Application.Interface.FilmInterface;
using Movie.Domain.Entities;
using Movie.Infrastructure.Database;

namespace Movie.Infrastructure.Repository.FilmRepo;

public interface IFilmRepo
{
    Task<List<Film>> GetFilmsAsync();
    Task<Film> GetFilmById(Guid id);
    Task<Film> FilterByGenre(Genre genre);
    Task<Film> FilterByReleaseYear(string year);
    Task<Film> GetFilmByTitle(string title);
    Task<int> GetRatingsCount(Guid movieId);
    Task<Film> AddMovieDetails(Film film);
    Task SaveChangesAsync();
}