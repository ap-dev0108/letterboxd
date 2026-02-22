using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Movie.Domain.Entities;
public class Film
{
    [Key]
    public Guid movieId {get; set;} = Guid.NewGuid();
    [Required]
    public string movieTitle {get; set;}
    [Required]
    public string releaseYear {get; set;}
    [Required]
    public int runTime {get; set;}
    [Required]
    public string? posterUrl {get; set;}
    [Required]
    public string? movieDescription {get; set;}
    public float AverageRatings {get;set;}
    public int TotalRatings {get;set;}
    public ICollection<Genre> MovieGenre {get;set;} = new List<Genre>();
    public ICollection<Ratings> MovieRatings {get; set;} = new List<Ratings>();
}