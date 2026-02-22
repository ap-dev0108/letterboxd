using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Domain.Entities;

public class Ratings
{
    [Key]
    public Guid RatingId {get; set;}
    [Required]
    [Range(1,5)]
    public float RatingScore {get;set;}
    public DateTime CreatedAt {get; set;} = DateTime.Now;

    [Required]
    public Guid MovieId {get; set;}
    public Film MovieRated {get; set;}
}
