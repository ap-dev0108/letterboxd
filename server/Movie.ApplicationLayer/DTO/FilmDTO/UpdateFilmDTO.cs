namespace Movie.Application.DTO.UpdateFilm;

public class UpdateFilmDTO
{
    public Guid movieId {get; set;}
    public string movieTitle {get; set;}
    public string releaseYear {get; set;}
    public string runTime {get; set;}
    public string? posterUrl {get; set;}
    public string? movieDescription {get; set;}
    public float AverageRatings {get;set;}
    public int TotalRatings {get;set;}
}