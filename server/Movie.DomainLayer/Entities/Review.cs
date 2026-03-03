using System.ComponentModel.DataAnnotations;

namespace Movie.Domain.Entities.Reviews;

public class Review
{
    [Key]
    public Guid ReviewId {get; set;} = Guid.NewGuid();

    [Required]
    public string userFeedback {get; set;} = string.Empty;

    public Guid UserId {get; set;}
    public Guid MovieId {get; set;}

    //Navigation Props
    public User user {get; set;}
    public Film film {get; set;}
}