using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Application.DTO.FilmDTO;
using Movie.Application.Interface.FilmInterface;
using Movie.Domain.Entities;
using Movie.Infrastructure.Database;

namespace Movie.Infrastructure.Repository.FilmRepo;

public class FilmRepo : IFilmRepo
{
    private readonly ApplicationDb _db;

    public FilmRepo(ApplicationDb db)
    {
        _db = db;
    }

    public async Task<List<Film>> GetFilmsAsync()
    {
        return await _db.Films.ToListAsync();
    }

    public async Task<Film> GetFilmById(Guid movieId)
    {
        return await _db.Films.FirstOrDefaultAsync(f => f.movieId == movieId);
    }

    public async Task<Film> FilterByGenre(Genre genre)
    {
        return await _db.Films.FirstOrDefaultAsync(f => f.MovieGenre == genre);
    }

    public async Task<Film> FilterByReleaseYear(string year)
    {
        return await _db.Films.FirstOrDefaultAsync(f => f.releaseYear == year);
    }

    public async Task<Film> GetFilmByTitle(string movieTitle)
    {
        return await _db.Films.FirstOrDefaultAsync(f => f.movieTitle == movieTitle);
    }
}
