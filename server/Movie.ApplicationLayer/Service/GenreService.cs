using Microsoft.AspNetCore.Razor.TagHelpers;
using Movie.Application.DTO.GenreDTO;
using Movie.Application.Interface.FilmInterface.GenreInterface;
using Movie.Application.Interface.RepoInterface.GenreRepo;
using Movie.Domain.Entities;
using Movie.Infrastructure.Database;
using Movie.Infrastructure.Repository.GenreRepo;

namespace Movie.Application.Service.GenreService;

public class GenreService : IGenreInterface
{
    private readonly IGenreRepo _genre;
    public GenreService(IGenreRepo genre)
    {
        _genre = genre;
    }

    public async Task<List<Genre>> GetGenresAsync()
    {
        try
        {
            var getGenres = await _genre.GetAllGenre();
            return getGenres;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<GenreDTO> AddGenre(GenreDTO genreDTO)
    {
        try
        {
            var getGenre = await _genre.FindGenreByTitle(genreDTO?.GenreTitle);
            if (getGenre != null) return null;

            var createdGenre = await _genre.AddGenre(genreDTO);
            return createdGenre;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<GenreDTO> UpdateGenreById(Guid updateId, GenreDTO genre)
    {
        try
        {
            var existingGenre = await _genre.GetGenreById(updateId); //Refactor need use instance insted
            if (existingGenre == null) return null;

            existingGenre.GenreTitle = genre.GenreTitle;

            await _genre.UpdateGenre(existingGenre);
            return new GenreDTO
            {
                GenreTitle = existingGenre.GenreTitle
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<GenreDTO> DeleteGenreById(GenreDTO genre)
    {
        try
        {
            var checkGenre = _genre.GetGenreById(genre.genreId);
            if (checkGenre == null) return new GenreDTO
            {
                genreId = genre.genreId,
                GenreTitle = "Data Null"
            };

            var genreDTO = new GenreDTO
            {
                genreId = genre.genreId,
                GenreTitle = genre.GenreTitle
            };

            // remove genre
            await _genre.DeleteGenre(genreDTO);
            return new GenreDTO
            {
                genreId = genre.genreId,
                GenreTitle = genre.GenreTitle
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}