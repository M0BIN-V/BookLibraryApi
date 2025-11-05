using BookLibraryApi.Dtos;
using BookLibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<CreatedResult> CreateAsync([FromBody] AddBookRequest request)
    {
        var result = await _bookService.AddAsync(request);

        return Created(string.Empty, result);
    }
}