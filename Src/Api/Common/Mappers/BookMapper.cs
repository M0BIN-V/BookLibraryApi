using Api.Dtos;
using Api.Models;

namespace Api.Common.Mappers;

public static class BookMapper
{
    public static GetBookResult ToViewDto(this Book book)
    {
        return new GetBookResult()
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear
        };
    }
}