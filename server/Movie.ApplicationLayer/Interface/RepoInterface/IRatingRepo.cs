using Movie.Domain.Entities;

namespace Movie.Application.Interface.RepoInterface.IRating;

public interface IRatingRepo
{
    Task<List<Ratings>> GetRatingsofMovies(Guid movieId);
    Task UpdateRatings(Ratings rating);
    Task AddRatings(Ratings ratings);
}