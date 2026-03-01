using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Application.DTO.AddFilm;
using Movie.Application.DTO.UpdateFilm;
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

    [Authorize(Roles = "Admin")]
    [HttpGet("/getAllMovies")]
    public async Task<IActionResult> GetFilmsAsync()
    {
        var getMovies = await _filmService.GetFilmsAsync();

        if(!getMovies.Any()) 
            return NoContent();

        return Ok(getMovies);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("/getMovieData")]
    public async Task<IActionResult> GetMovieData(Guid movieId)
    {
        var getMovie = await _filmService.ShowMovieDetails(movieId);
        return Ok(new APIResponse<Film>
        {
            Success = true,
            Message = "Movie fetched",
            Data = getMovie
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("/addMovies")]
    public async Task<IActionResult> AddMovies(AddFilmDTO moviesData)
    {
        var movie = await _filmService.AddMovieDetails(moviesData);

        return Ok(new APIResponse<Film>
        {
            Success = true,
            Message = "Movies added successfully",
            Data = movie  
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("getTotalRatings")]
    public async Task<IActionResult> GetRatingCount(Guid movieId)
    {
        var getRatings = await _filmService.GetTotalRatings(movieId);

        return Ok(new APIResponse<string>
        {
            Success = true,
            Message = "Rating Count Fetched",
            Data = $"{getRatings} ratings so far"
        });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("update-movies")]
    public async Task<IActionResult> UpdateMovies(Guid movieId, UpdateFilmDTO updateFilmDTO)
    {

        await _filmService.UpdateFilm(movieId, updateFilmDTO);
        return Ok (new APIResponse<string>
        {
            Success = true,
            Message = "Movie has been updated",
            Data = $"The movie with the following ID: {movieId} has been updated"
        });
    }
}