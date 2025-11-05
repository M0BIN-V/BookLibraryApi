using BookLibraryApi.Common.Errors;
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

    public async Task<OneOf<BookNotFoundError,string>> UpdateAsync(int bookId , UpdateBookDto updateBook)
    {
        var book = await _dbContext.Books.FindAsync(bookId);
        
        if(book is null) return new BookNotFoundError();
        
        book.Title = updateBook.Title;
        book.Author = updateBook.Author;
        book.Genre = updateBook.Genre;
        book.PublishedYear = updateBook.PublishedYear;
        
        await _dbContext.SaveChangesAsync();
        
        return "Book updated successfully.";
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        return _dbContext.Books.SingleOrDefaultAsync(b => b.Id == id);
    }

    public Task<List<Book>> GetAll(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<Book> AddAsync(AddBookRequest book)
    {
        var newBook = new Book
        {
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear,
        };
        
        await _dbContext.Books.AddAsync(newBook);
        await _dbContext.SaveChangesAsync();

        return newBook;
    }
}
