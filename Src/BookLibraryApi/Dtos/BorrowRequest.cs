using System.ComponentModel.DataAnnotations;

namespace BookLibraryApi.Dtos;

public class BorrowRequest
{
    [Required] [Range(1, int.MaxValue)]
    public int UserId { get; init; }
}