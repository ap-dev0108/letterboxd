using Microsoft.AspNetCore.Mvc;
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
        try
        {
            var getMovies = await _filmService.GetFilmsAsync();
            return Ok(getMovies);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("/getMovieData")]
    public async Task<IActionResult> GetMovieData(Film movie)
    {
        try
        {
            var getMovie = await _filmService.ShowMovieDetails(movie);
            return Ok(getMovie);
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("/addMovies")]
    public async Task<IActionResult> AddMovies(Film moviesData)
    {
        try
        {
            var addMovie = await _filmService.AddMovieDetails(moviesData);
            return Ok(addMovie);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}