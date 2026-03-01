using System.Linq;
using Movie.Application.DTO.AddFilm;
using Movie.Application.DTO.FilmDTO;
using Movie.Application.DTO.UpdateFilm;
using Movie.Application.Interface.FilmInterface;
using Movie.Domain.Entities;
using Movie.Infrastructure.Repository.FilmRepo;

namespace Movie.Application.Service;

public class FilmService : IFilmInterface
{
    private readonly IFilmRepo _filmRepo;

    public FilmService(IFilmRepo filmRepo)
    {
        _filmRepo = filmRepo;
    }

    public async Task<List<AddFilmDTO>> GetFilmsAsync()
    {
        try
        {
            var getFilms = await _filmRepo.GetFilmsAsync() ?? 
                throw new KeyNotFoundException("Films are null");
            
            var filmDTO = getFilms.Select(f => new AddFilmDTO
            {
                movieId = f.movieId,
                movieTitle = f.movieTitle,
                movieDescription = f.movieDescription,
                posterUrl = f.posterUrl,
                releaseYear = f.releaseYear,
                runTime = f.runTime,
                AverageRatings = f.AverageRatings,
                TotalRatings = f.TotalRatings
            }).ToList();

            return filmDTO;
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Film> FilterByGenre(Genre genre)
    {
        try
        {
            var filterMovie = await _filmRepo.FilterByGenre(genre);
            return filterMovie;
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Film> FilterByReleaseYear(string year)
    {
        try
        {
            var checkYear = await _filmRepo.FilterByReleaseYear(year) ??
                throw new KeyNotFoundException("Movie not found");

            return checkYear;
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Film> ShowMovieDetails(Guid movieId)
    {
        try
        {
            var checkMovie = await _filmRepo.GetFilmById(movieId);
            if (checkMovie == null) 
                throw new KeyNotFoundException("Cannot find the matching movie ID");

            var movie = new Film
            {
                movieId = checkMovie.movieId,
                movieTitle = checkMovie.movieTitle,
                movieDescription = checkMovie.movieDescription,
                releaseYear = checkMovie.releaseYear,
                runTime = checkMovie.runTime,
                posterUrl = checkMovie.posterUrl,
                AverageRatings = checkMovie.AverageRatings,
                TotalRatings = checkMovie.TotalRatings,
                MovieGenre = checkMovie.MovieGenre,
                MovieRatings = checkMovie.MovieRatings
            };

            return movie;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Film> AddMovieDetails(AddFilmDTO movie)
    {
        try
        {
            var fetchMovieTitle = await _filmRepo.GetFilmByTitle(movie.movieTitle) ??
                throw new KeyNotFoundException("Movie with the title not found");

            var addMovie = new Film
            {
                movieTitle = movie.movieTitle,
                movieDescription = movie.movieDescription,
                posterUrl = movie.posterUrl,
                releaseYear = movie.releaseYear,
                runTime = movie.runTime,
                AverageRatings = movie.AverageRatings,
                TotalRatings = movie.TotalRatings
            };

            var movies = await _filmRepo.AddMovieDetails(addMovie) ?? 
                throw new UnauthorizedAccessException("The movie details entered are invalid");
                
            return movies;
        } 
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<int> GetTotalRatings(Guid movieId)
    {
        try
        {
            var getMovie = await _filmRepo.GetFilmById(movieId) ?? 
                throw new KeyNotFoundException("Movie cannot be found");

            var getTotalRatings = await _filmRepo.GetRatingsCount(movieId);
            return getTotalRatings;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Film> UpdateFilm(Guid movieId, UpdateFilmDTO updateFilmDTO)
    {
        try
        {
            var existingMovie = await _filmRepo.GetFilmById(movieId);
            if (existingMovie == null) throw new KeyNotFoundException($"Movie with ID: {movieId} does not exist");

            existingMovie.movieTitle = updateFilmDTO.movieTitle;
            existingMovie.movieDescription = updateFilmDTO.movieDescription;
            existingMovie.posterUrl = updateFilmDTO.posterUrl;
            existingMovie.releaseYear = updateFilmDTO.releaseYear;
            existingMovie.runTime = updateFilmDTO.runTime;
            existingMovie.AverageRatings = updateFilmDTO.AverageRatings;
            existingMovie.TotalRatings = updateFilmDTO.TotalRatings;

            await _filmRepo.SaveChangesAsync();
            return existingMovie;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    } 
}