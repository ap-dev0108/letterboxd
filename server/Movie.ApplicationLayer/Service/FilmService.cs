using System.Linq;
using Movie.Application.DTO.FilmDTO;
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

    public async Task<List<FilmDisplayDTO>> GetFilmsAsync()
    {
        try
        {
            var getFilms = await _filmRepo.GetFilmsAsync();
            
            var filmDTO = getFilms.Select(f => new FilmDisplayDTO
            {
                movieId = f.movieId,
                movieName = f.movieTitle
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
            var checkYear = await _filmRepo.FilterByReleaseYear(year);
            if (checkYear == null) return null;

            return checkYear;

        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<FilmDataDTO> ShowMovieDetails(Film movieData)
    {
        try
        {
            var checkMovie = await _filmRepo.GetFilmById(movieData.movieId);
            if (checkMovie == null) return null;

            var movieDTO = new FilmDataDTO
            {
                filmOverview = new FilmDisplayDTO
                {
                    movieId = checkMovie.movieId,
                    movieName = checkMovie.movieTitle,
                    posterURL = checkMovie.posterUrl
                },
                filmDescription = checkMovie.movieDescription,
                runTime = checkMovie.runTime,
                releaseYear = checkMovie.releaseYear,
                genres = checkMovie.MovieGenre
            };

            return movieDTO;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Film> AddMovieDetails(Film movie)
    {
        try
        {
            var fetchMovieTitle = await _filmRepo.GetFilmByTitle(movie.movieTitle);
            if (fetchMovieTitle != null) return null;

            var addMovies = new Film
            {
                movieTitle = movie.movieTitle,
                movieDescription = movie.movieDescription,
                posterUrl = movie.posterUrl,
                MovieGenre = movie.MovieGenre,
                releaseYear = movie.releaseYear,
                runTime = movie.runTime
            };

            return addMovies;   
        } 
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}