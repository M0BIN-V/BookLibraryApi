using BookLibraryApi.Common.Errors;
using BookLibraryApi.Common.Mappers;
using BookLibraryApi.Data;
using BookLibraryApi.Dtos;
using BookLibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Services;

public class BookService : IBookService
{
    readonly AppDbContext _dbContext;

    public BookService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OneOf<BookNotFoundError, string>> RemoveAsync(int bookId)
    {
        var deletedCount = await _dbContext.Books
            .Where(b => b.Id == bookId)
            .ExecuteDeleteAsync();

        return deletedCount < 1 ? new BookNotFoundError() : "Book removed successfully.";
    }

    public async Task<OneOf<BookNotFoundError, string>> UpdateAsync(int bookId, UpdateBookRequest updateBook)
    {
        var book = await _dbContext.Books.FindAsync(bookId);

        if (book is null) return new BookNotFoundError();

        book.Title = updateBook.Title;
        book.Author = updateBook.Author;
        book.Genre = updateBook.Genre;
        book.PublishedYear = updateBook.PublishedYear;

        await _dbContext.SaveChangesAsync();

        return "Book updated successfully.";
    }

    public Task<GetBookResult?> GetByIdAsync(int id)
    {
        return _dbContext.Books.Select(c => new GetBookResult()
            {
                Id = c.Id,
                Title = c.Title,
                Author = c.Author,
                Genre = c.Genre,
                PublishedYear = c.PublishedYear
            })
            .AsNoTracking()
            .SingleOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<GetBookResult>> GetAll(int pageNumber, int pageSize)
    {
        var result = await _dbContext.Books
            .FromSqlRaw("EXEC GetAllBooks @PageNumber={0}, @PageSize={1}", pageNumber, pageSize)
            .ToListAsync();

        return result.Select(r => r.ToViewDto()).ToList();
    }

    public async Task<GetBookResult> AddAsync(AddBookRequest book)
    {
        var newBook = new Book
        {
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear
        };

        await _dbContext.Books.AddAsync(newBook);
        await _dbContext.SaveChangesAsync();

        return newBook.ToViewDto();
    }
}