using BookLibraryApi.Common.Errors;
using BookLibraryApi.Common.Mappers;
using BookLibraryApi.Data;
using BookLibraryApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Services;

public class BorrowService : IBorrowService
{
    readonly AppDbContext _dbContext;

    public BorrowService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OneOf<
        UserNotFoundError,
        BookNotFoundError,
        BookAlreadyBorrowedError,
        BorrowResult>> BorrowBook(int bookId, int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user is null) return new UserNotFoundError();

        var book = await _dbContext.Books.FindAsync(bookId);
        if (book is null) return new BookNotFoundError();
        
        var borrowedAt = DateTimeOffset.UtcNow;
        
        var affected = await _dbContext.Books
            .Where(b => b.Id == bookId && b.BorrowedById == null)
            .ExecuteUpdateAsync(b => b
                .SetProperty(x => x.BorrowedById, userId)
                .SetProperty(x => x.BorrowedAt, borrowedAt));

        if (affected is 0) return new BookAlreadyBorrowedError(book.Title);
        
        return new BorrowResult(book.ToViewDto(), borrowedAt, user.ToVewUserDto());
    }

    public async Task<OneOf<
        BookNotFoundError,
        BookNotBorrowedByUserError,
        string>> ReturnBook(int bookId, int userId)
    {
        var book = await _dbContext.Books.FindAsync(bookId);
        if (book is null) return new BookNotFoundError();

        if (book.BorrowedById != userId) return new BookNotBorrowedByUserError();

        book.BorrowedById = null;
        book.BorrowedAt = null;

        await _dbContext.SaveChangesAsync();

        return "Book returned successfully";
    }
}