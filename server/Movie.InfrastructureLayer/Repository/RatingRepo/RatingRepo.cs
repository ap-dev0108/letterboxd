using Microsoft.EntityFrameworkCore;
using Movie.Application.Interface.RepoInterface.IRating;
using Movie.Domain.Entities;
using Movie.Infrastructure.Database;

namespace Movie.Infrastructure.Repository.RatingRepo;

public class RatingRepo : IRatingRepo
{
    private readonly ApplicationDb _db;

    public RatingRepo(ApplicationDb db)
    {
        _db = db;
    }

    public async Task<List<Ratings>> GetRatingsofMovies(Guid movieId)
    {
        return await _db.Ratings.Where(x => x.MovieId == movieId).ToListAsync();
    }

    public async Task UpdateRatings(Ratings rating)
    {
        _db.Ratings.Update(rating);
        _db.SaveChanges();
    }

    public async Task AddRatings(Ratings ratings)
    {
        _db.Ratings.Add(ratings);
        _db.SaveChanges();
    }
}