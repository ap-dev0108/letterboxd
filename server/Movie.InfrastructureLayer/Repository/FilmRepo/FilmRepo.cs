using Microsoft.EntityFrameworkCore;
using Movie.Application.DTO.FilmDTO.FilmFilter;
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
        return await _db.Films.AsNoTracking().ToListAsync();
    }

    public async Task<Film> GetFilmById(Guid movieId)
    {
        return await _db.Films.AsNoTracking().FirstOrDefaultAsync(f => f.movieId == movieId);
    }
    public async Task<List<Film>> GetFilmByTitle(string movieTitle)
    {
        return await _db.Films
        .Where(m => m.movieTitle.ToLower().Contains(movieTitle))
        .ToListAsync();
    }

    public async Task<int> GetRatingsCount(Guid movieId)
    {
        var getMovies = await _db.Films.AsNoTracking().FirstOrDefaultAsync(f => f.movieId == movieId);
        return getMovies.TotalRatings;
    }

    public async Task<Film> AddMovieDetails(Film film)
    {
        await _db.Films.AddAsync(film);
        return film;
    }

    public async Task<(float MinRating, float MaxRating)> GetMinMaxRating(Film film)
    {
        var results = await _db.Films.Where(w => w.movieId == film.movieId)
        .Select(f => new
        {
            MinRating = f.MovieRatings.Min(r => r.RatingScore),
            MaxRating = f.MovieRatings.Max(r => r.RatingScore)
        }).FirstOrDefaultAsync();

        return (results.MinRating, results.MaxRating);
    }

    public async Task<List<Film>> GetFilmFilteredAsync(FilmFilterDto filmFilterDto)
    {
        var query = _db.Films
            .Include(f => f.MovieGenre)
            .Include(f => f.MovieRatings)
            .AsQueryable();

        // Filter by genre (many-to-many collection)
        if (filmFilterDto.GenreId.HasValue)
        {
            var genreId = filmFilterDto.GenreId.Value;
            query = query.Where(f => f.MovieGenre.Any(g => g.GenreId == genreId));
        }

        // Filter by release year
        if (!string.IsNullOrWhiteSpace(filmFilterDto.ReleaseYear))
        {
            query = query.Where(f => f.releaseYear == filmFilterDto.ReleaseYear);
        }

        // Filter by rating range using the cached AverageRatings field
        if (filmFilterDto.MinRating.HasValue)
        {
            query = query.Where(f => f.AverageRatings >= filmFilterDto.MinRating.Value);
        }

        if (filmFilterDto.MaxRating.HasValue)
        {
            query = query.Where(f => f.AverageRatings <= filmFilterDto.MaxRating.Value);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task DeleteMovie(Film film)
    {
        _db.Films.Remove(film);
    }

    //Method for saving database changes. 
    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
