using System.ComponentModel.DataAnnotations;
using BookLibraryApi.Models.Abstraction;

namespace BookLibraryApi.Models;

public class Book : EntityBase
{
    public required string  Title { get; set; }
    public required string Author { get; set; }
    public required string Genre { get; set; }
    public required int PublishedYear { get; set; }
    
    public BorrowRecord? CurrentBorrow { get; set; }
    
    public byte[] RowVersion { get; set; } = [];
}