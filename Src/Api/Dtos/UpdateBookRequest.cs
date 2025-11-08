using System.ComponentModel.DataAnnotations;
using Api.Common.Validators;

namespace Api.Dtos;

public class UpdateBookRequest
{
    [Required] [StringLength(50)]
    public required string Title { get; set; }

    [Required] [StringLength(50)]
    public required string Author { get; set; }

    [Required] [StringLength(50)]
    public required string Genre { get; set; }

    [ValidPublishedYear]
    public required int PublishedYear { get; set; }
}