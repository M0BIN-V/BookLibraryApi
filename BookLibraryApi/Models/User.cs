using Azure.Core;
using BookLibraryApi.Models.Abstraction;

namespace BookLibraryApi.Models;

public class User : EntityBase
{
    public required string Name { get; set; }
    public required string Email{ get; set; }
    
    public ICollection<BorrowRecord> BorrowedBooks { get; set; } = [];
}