using Microsoft.AspNetCore.Mvc;
using Movie.Application.DTO.GenreDTO;
using Movie.Application.Service.GenreService;
using Movie.Domain.Entities;

namespace Movie.Presentation.Controller.GenreController;

[Route("[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly GenreService _genreServices;
    public GenreController(GenreService genreServices)
    {
        _genreServices = genreServices;
    }

    [HttpGet("/getAllGenre")]
    public async Task<APIResponse<List<Genre>>> GetAllGenres()
    {
        try
        {
            var getGenres = await _genreServices.GetGenresAsync();
            if (getGenres.Count == 0) return new()
            {
                Success = true,
                Message = "No Content Found.",
                Data = getGenres
            };

            return new()
            {
                Success = true,
                Message = "Data Found",
                Data = getGenres
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }

    [HttpPost("/addGenres")]
    public async Task<APIResponse<GenreDTO>> AddGenre(GenreDTO genres)
    {
        try
        {
            var addGenre = await _genreServices.AddGenre(genres);
            return new ()
            {
                Success = true,
                Message = "Genre Added",
                Data = addGenre
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }

    [HttpPut("/updateGenres")]
    public async Task<APIResponse<GenreDTO>> UpdateGenre(Guid genreID, GenreDTO genreDTO)
    {
        try
        {
            var updateGenre = await _genreServices.UpdateGenreById(genreID, genreDTO);
            return new ()
            {
                Success = true,
                Message = "Genre is Updated",
                Data = updateGenre  
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                Success = false,
                Message = ex.Message,
                Data = genreDTO
            };
        }
    }

    [HttpDelete("/deleteGenre")]
    public async Task<APIResponse<GenreDTO>> DeleteGenre(GenreDTO genreDTO)
    {
        try
        {
            var deleteGenre = await _genreServices.DeleteGenreById(genreDTO);
            return new ()
            {
                Success = true,
                Message = "Genre is Deleted",
                Data = genreDTO
            };
        }
        catch (Exception ex)
        {
            return new ()
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }
}