using BookLibraryApi.Dtos;
using BookLibraryApi.Models;

namespace BookLibraryApi.Common.Mappers;

public static class BookMapper
{
    public static ViewBookDto ToViewDto(this Book book)
    {
        return new ViewBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear
        };
    }
}