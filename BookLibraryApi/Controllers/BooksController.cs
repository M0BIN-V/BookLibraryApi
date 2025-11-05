using System.ComponentModel.DataAnnotations;
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

    [HttpGet]
    [ProducesResponseType<List<ViewBookDto>>(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] [Range(1, int.MaxValue)] int pageNumber,
        [FromQuery] [Range(1,100)]int pageSize)
    {
        var result = await _bookService.GetAll(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<ViewBookDto>(Status200OK)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null) return NotFound();

        return Ok(book);
    }

    [HttpPost]
    [ProducesResponseType<ViewBookDto>(Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] AddBookRequest request)
    {
        var book = await _bookService.AddAsync(request);

        return CreatedAtAction("Get", new { id = book.Id }, book);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status404NotFound)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateBookDto request)
    {
        var result = await _bookService.UpdateAsync(id, request);

        return result.Match<StatusCodeResult>(
            notfoundError => NotFound(),
            successMessage => Ok()
        );
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(Status204NoContent)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _bookService.RemoveAsync(id);

        return result.Match<StatusCodeResult>(
            notfoundError => NotFound(),
            successMessage => NoContent()
        );
    }
}