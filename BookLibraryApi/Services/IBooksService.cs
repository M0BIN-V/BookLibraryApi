using BookLibraryApi.Common.Errors;
using BookLibraryApi.Dtos;
using BookLibraryApi.Models;

namespace BookLibraryApi.Services;

public interface IBookService
{
    public Task<OneOf<BookNotFoundError, string>> RemoveAsync(int bookId);
    public Task<OneOf<BookNotFoundError, string>> UpdateAsync(int bookId, UpdateBookDto updateBook);

    public Task<ViewBookDto?> GetByIdAsync(int id);

    public Task<List<ViewBookDto>> GetAll(int pageNumber, int pageSize);

    public Task<ViewBookDto> AddAsync(AddBookRequest book);
}