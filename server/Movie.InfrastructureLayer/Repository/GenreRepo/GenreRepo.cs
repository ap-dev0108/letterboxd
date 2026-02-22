using Microsoft.EntityFrameworkCore;
using Movie.Application.DTO.GenreDTO;
using Movie.Application.Interface.RepoInterface.GenreRepo;
using Movie.Domain.Entities;
using Movie.Infrastructure.Database;

namespace Movie.Infrastructure.Repository.GenreRepo;

public class GenreRepo : IGenreRepo
{
    private readonly ApplicationDb _db;
    
    public GenreRepo(ApplicationDb db)
    {
        _db = db;
    }

    public async Task<List<Genre>> GetAllGenre()
    {
        return await _db.Genres.ToListAsync();
    }

    public async Task<Genre> FindGenreByTitle(string title)
    {
        return await _db.Genres.FirstOrDefaultAsync(f => f.GenreTitle == title);
    }

    public async Task<Genre> GetGenreById(Guid genreId)
    {
        return await _db.Genres.FirstOrDefaultAsync(g => g.GenreId == genreId);
    }

    public async Task<GenreDTO> AddGenre(GenreDTO genreDTO)
    {
        var genre = new Genre
        {
            GenreTitle = genreDTO.GenreTitle
        };

        await _db.Genres.AddAsync(genre);
        _db.SaveChanges();

        return new GenreDTO
        {
            GenreTitle = genre.GenreTitle
        };
    }
    public async Task UpdateGenre(Genre genre)
    {
        _db.Genres.Update(genre);
        _db.SaveChanges();
    }

    public async Task DeleteGenre(GenreDTO genreDTO)
    {
        var deletingGenre = new Genre
        {
            GenreId = genreDTO.genreId,
            GenreTitle = genreDTO.GenreTitle
        };

        _db.Genres.Remove(deletingGenre);
        _db.SaveChanges();
    }
}