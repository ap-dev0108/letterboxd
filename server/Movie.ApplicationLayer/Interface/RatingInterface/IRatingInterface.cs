using Movie.Application.DTO.FilmDTO.RatingsDTO;
using Movie.Domain.Entities;

namespace Movie.Application.Interface.RatingInterface;

public interface IRatingInterface
{
    Task<List<Ratings>> GetRatingsByMovies(Guid movieId);
    Task<Ratings> UpdateMovieRatings(Guid movieId, float rating);
    Task<Ratings> AddMovieRatings(Guid movieId, float rating);
}