using Movie.Domain.Entities;

namespace Movie.Application.DTO.FilmDTO;

public class FilmDataDTO
{
    public FilmDisplayDTO filmOverview {get; set;}
    public string? filmDescription {get; set;}
    public string runTime {get; set;}
    public string? releaseYear {get; set;}
    public ICollection<Genre> genres {get; set;}
}

public class FilmDisplayDTO
{
    public Guid movieId {get; set;}
    public string movieName {get; set;}
    public string? posterURL {get; set;}
}