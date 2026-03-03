using Movie.Domain.Entities;

namespace Movie.Application.DTO.FilmDTO.FilmFilter;

public class FilmFilterDto
{
    public Guid? GenreId { get; set; }
    public string? ReleaseYear { get; set; }
    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }
}