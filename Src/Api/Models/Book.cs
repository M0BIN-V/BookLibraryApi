using Api.Models.Abstraction;

namespace Api.Models;

public class Book : EntityBase
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Genre { get; set; }
    public required int PublishedYear { get; set; }
    
    public DateTimeOffset? BorrowedAt { get; set; }
    public int? BorrowedById { get; set; }
    public User? BorrowedBy{ get; set; } 
}