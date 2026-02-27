using Microsoft.AspNetCore.Mvc;
using Movie.Application.Service.RatingService;
using Movie.Domain.Entities;

namespace Movie.Presentation.Controller.RatingController;

[ApiController]
[Route("[controller]")]
public class RatingController : ControllerBase
{
    private readonly RatingService _ratingService;

    public RatingController(RatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpGet("getRatingsByMovie")]
    public async Task<APIResponse<List<Ratings>>> GetMovieRatings(Guid id)
    {
        try
        {
            var getMovieRatings = await _ratingService.GetRatingsByMovies(id);

            if (getMovieRatings == null) return new ()
            {
                Success = false,
                Message = "Movie Ratings are null",
                Data = null
            };

            return new ()
            {
                Success = true,
                Message = "Ratings Fetched",
                Data = getMovieRatings.ToList()
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

    [HttpPut("change-ratings")]
    public async Task<APIResponse<Ratings>> ChangeRatings(Guid movieId, float rating)
    {
        try
        {
            var updateRatings = await _ratingService.UpdateMovieRatings(movieId, rating);

            return new ()
            {
                Success = true,
                Message = "Ratings Updated",
                Data = updateRatings
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

    [HttpPost("add-ratings")]
    public async Task<IActionResult> AddRatings(Guid movieId, float rating)
    {
        var addRatings = await _ratingService.AddMovieRatings(movieId, rating);

        if (addRatings == null) return NoContent();

        return Ok(new APIResponse<Ratings>
        {
            Success = true,
            Message = "Rating is added",
            Data = addRatings
        });
    }
}