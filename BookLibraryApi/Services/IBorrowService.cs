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
}