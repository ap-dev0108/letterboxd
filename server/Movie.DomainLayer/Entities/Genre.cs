using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movie.Application.Service;

namespace Movie.Domain.Entities;

public class Genre
{
    [Key]
    public Guid GenreId {get; set;} = Guid.NewGuid();

    [Required]
    public string GenreTitle {get; set;} = string.Empty;

    public ICollection<Film>? Films {get; set;} = [];
}