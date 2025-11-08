using System.ComponentModel.DataAnnotations;

namespace BookLibraryApi.Dtos;

public class ReturnRequest
{
    [Required] [Range(1, int.MaxValue)]
    public int UserId { get; init; }
}