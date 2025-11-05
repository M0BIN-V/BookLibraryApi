namespace BookLibraryApi.Dtos;

public class ViewBookDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }

    public required string Author { get; set; }

    public required string Genre { get; set; }

    public required int PublishedYear { get; set; }
}