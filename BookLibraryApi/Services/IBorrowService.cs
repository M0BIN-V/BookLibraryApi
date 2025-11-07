using BookLibraryApi.Common.Errors;
using BookLibraryApi.Dtos;

namespace BookLibraryApi.Services;

public interface IBorrowService
{
    public Task<OneOf<
        UserNotFoundError,
        BookNotFoundError,
        BookAlreadyBorrowedError,
        BorrowResult>> BorrowBook(int bookId, int userId);

    public Task<OneOf<
        BookNotFoundError,
        BookNotBorrowedByUserError,
        string>> ReturnBook(int bookId, int userId);
}