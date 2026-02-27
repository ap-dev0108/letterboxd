using Microsoft.AspNetCore.Mvc;
using Movie.Application.DTO.AddFilm;
using Movie.Application.Service;
using Movie.Domain.Entities;

namespace Movie.Presentation.Controller;

[Route("[controller]")]
[ApiController]
public class FilmController : ControllerBase
{
    private readonly FilmService _filmService;
    public FilmController(FilmService filmService)
    {
        _filmService = filmService;
    }

    [HttpGet("/getAllMovies")]
    public async Task<IActionResult> GetFilmsAsync()
    {
        var getMovies = await _filmService.GetFilmsAsync();

        if(!getMovies.Any()) 
            return NoContent();

        return Ok(getMovies);
    }

    [HttpGet("/getMovieData")]
    public async Task<APIResponse<Film>> GetMovieData(Guid movieId)
    {
        try
        {
            var getMovie = await _filmService.ShowMovieDetails(movieId);
            return new ()
            {
                Success = true,
                Message = "Movie fetched",
                Data = getMovie
            };
        }

        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = $"Controller Exception : {ex.Message}"
            };
        }
    }

    [HttpPost("/addMovies")]
    public async Task<APIResponse<Film>> AddMovies(AddFilmDTO moviesData)
    {
        try
        {
            var movie = await _filmService.AddMovieDetails(moviesData);

            return new ()
            {
                Success = true,
                Message = "Movies added successfully",
                Data = movie  
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = $"Controller Exception: {ex.Message}"
            };
        }
    }

    [HttpGet("getTotalRatings")]
    public async Task<APIResponse<string>> GetRatingCount(Guid movieId)
    {
        try
        {
            var getRatings = await _filmService.GetTotalRatings(movieId);
            if (getRatings == null) return new ()
            {
                Success = false,
                Message = "Count is null",
            };

            return new ()
            {
                Success = true,
                Message = "Rating Count Fetched",
                Data = $"{getRatings} ratings so far"
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = $"Controller Exception: {ex.Message}"
            };
        }
    }

}