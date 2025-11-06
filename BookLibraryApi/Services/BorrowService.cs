using BookLibraryApi.Common.Errors;
using BookLibraryApi.Common.Mappers;
using BookLibraryApi.Data;
using BookLibraryApi.Dtos;
using BookLibraryApi.Models;
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

        var book = await _dbContext.Books
            .SingleOrDefaultAsync(b => b.Id == bookId);
        if (book is null) return new BookNotFoundError();

        if (book.BorrowedById is not null) return new BookAlreadyBorrowedError(book.Title);
        
        book.BorrowedById = user.Id;
        book.BorrowedAt = DateTimeOffset.UtcNow;
        
        await _dbContext.SaveChangesAsync();

        return new BorrowResult(book.ToViewDto(), book.BorrowedAt, user.ToVewUserDto());
    }
}