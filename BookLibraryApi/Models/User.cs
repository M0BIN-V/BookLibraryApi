using BookLibraryApi.Models.Abstraction;

namespace BookLibraryApi.Models;

public class User : EntityBase
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}