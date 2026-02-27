namespace Movie.Application.DTO.FilmDTO.RatingsDTO;

public class RatingsDTO
{
    public float RatingScore {get; set;}
    public Guid MovieId {get; set;}
    public DateTime UpdatedTime {get; set;}
}