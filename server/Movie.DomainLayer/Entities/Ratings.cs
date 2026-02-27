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
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    [Required]
    public Guid MovieId {get; set;}
    public ICollection<Film>? Films {get; set;} = [];

    [Required]
    public Guid UserId {get; set;}
}
