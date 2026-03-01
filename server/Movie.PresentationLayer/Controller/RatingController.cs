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
    public async Task<IActionResult> GetMovieRatings(Guid id)
    {
        var getMovieRatings = await _ratingService.GetRatingsByMovies(id);

        if (!getMovieRatings.Any()) return NoContent();

        return Ok(new APIResponse<List<Ratings>>
        {
            Success = true,
            Message = "Ratings Fetched",
            Data = getMovieRatings.ToList()
        });
    }

    [HttpPut("change-ratings")]
    public async Task<IActionResult> ChangeRatings(Guid movieId, float rating)
    {
        var updateRatings = await _ratingService.UpdateMovieRatings(movieId, rating);
    
        return Ok(new APIResponse<Ratings>
        {
            Success = true,
            Message = "Ratings Updated",
            Data = updateRatings
        });
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