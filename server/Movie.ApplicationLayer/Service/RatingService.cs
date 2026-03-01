using Movie.Application.Interface.RatingInterface;
using Movie.Application.Interface.RepoInterface.IRating;
using Movie.Domain.Entities;
using Movie.Infrastructure.Repository.FilmRepo;
using Movie.Application.DTO.FilmDTO.RatingsDTO;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Movie.Application.Interface.RepoInterface.UserRepo;

namespace Movie.Application.Service.RatingService;

public class RatingService : IRatingInterface
{
    private IRatingRepo _ratingRepo;
    private IFilmRepo _filmRepo;
    private IUserRepo _userRepo;

    public RatingService(IRatingRepo ratingRepo, IFilmRepo filmRepo, IUserRepo userRepo)
    {
        _ratingRepo = ratingRepo;
        _filmRepo = filmRepo;
        _userRepo = userRepo;
    }

    public async Task<List<Ratings>> GetRatingsByMovies(Guid movieId)
    {
        try{
            var getMovies = await _filmRepo.GetFilmById(movieId) ??
                throw new KeyNotFoundException("Movies cannot be found");

            var getRatings = await _ratingRepo.GetRatingsofMovies(movieId) ??
                throw new KeyNotFoundException("Ratings cannot be found");

            return getRatings;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Ratings> UpdateMovieRatings(Guid movieId, float rating)
    {
        try
        {
            //make a service which get's token data and retrive the id then use it to get users
            var movie = await _filmRepo.GetFilmById(movieId) ?? 
                throw new KeyNotFoundException("Movie cannot be found");

            var ratings = new Ratings
            {
                RatingId = Guid.NewGuid(),
                RatingScore = rating,
                MovieId = movieId,
                CreatedAt = DateTime.UtcNow
            };
            
            await _ratingRepo.UpdateRatings(ratings);
            return ratings;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Ratings> AddMovieRatings(Guid movieId, float rating)
    {
        try
        {
            //can make this thing littel short by handling null check in line 76
            var movieExists = await _filmRepo.GetFilmById(movieId) ?? 
                throw new KeyNotFoundException("Movie cannot be found");  

            var addRatings = new Ratings
            {
                RatingScore = rating,
                MovieId = movieId
            };

            movieExists.TotalRatings += 1;

            movieExists.AverageRatings = ((movieExists.AverageRatings * (movieExists.TotalRatings - 1)) + rating) / movieExists.TotalRatings;

            await _ratingRepo.AddRatings(addRatings);
            await _filmRepo.SaveChangesAsync();
            return addRatings;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}