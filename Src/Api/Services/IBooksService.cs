using Api.Common.Errors;
using Api.Dtos;
using Api.Models;

namespace Api.Services;

public interface IBookService
{
    public Task<OneOf<BookNotFoundError, string>> RemoveAsync(int bookId);
    public Task<OneOf<BookNotFoundError, string>> UpdateAsync(int bookId, UpdateBookRequest updateBook);

    public Task<GetBookResult?> GetByIdAsync(int id);

    public Task<List<GetBookResult>> GetAll(int pageNumber, int pageSize);

    public Task<GetBookResult> AddAsync(AddBookRequest book);
}