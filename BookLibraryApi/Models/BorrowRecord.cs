using BookLibraryApi.Models.Abstraction;

namespace BookLibraryApi.Models;

public class BorrowRecord : EntityBase
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}