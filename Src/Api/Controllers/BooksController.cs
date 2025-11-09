using System.ComponentModel.DataAnnotations;
using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable UnusedParameter.Local
// ReSharper disable ConvertClosureToMethodGroup

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    readonly IBookService _bookService;
    readonly IBorrowService _borrowService;

    public BooksController(IBookService bookService, IBorrowService borrowService)
    {
        _bookService = bookService;
        _borrowService = borrowService;
    }

    [HttpPost("{id:int}/return")]
    [ProducesResponseType<BorrowResult>(Status204NoContent)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> Return(int id, [FromBody] BorrowRequest request)
    {
        var result = await _borrowService.ReturnBook(id, request.UserId);

        return result.Match<IActionResult>(
            bookNotFound => NotFound(),
            bookNotBorrowedByUserError => BadRequest(bookNotBorrowedByUserError),
            success => NoContent());
    }

    [HttpPost("{id:int}/borrow")]
    [ProducesResponseType<BorrowResult>(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> Borrow(int id, [FromBody] BorrowRequest request)
    {
        var result = await _borrowService.BorrowBook(id, request.UserId);

        return result.Match<IActionResult>(
            userNotFount => NotFound(),
            bookNotFound => NotFound(),
            bookAlreadyBorrowed => BadRequest(bookAlreadyBorrowed),
            successResult => Ok(successResult));
    }

    [HttpGet]
    [ProducesResponseType<List<GetBookResult>>(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] [Required] [Range(1, int.MaxValue)]
        int pageNumber,
        [FromQuery] [Required] [Range(1, 100)] int pageSize)
    {
        var result = await _bookService.GetAll(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<GetBookResult>(Status200OK)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null) return NotFound();

        return Ok(book);
    }

    [HttpPost]
    [ProducesResponseType<GetBookResult>(Status201Created)]
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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateBookRequest request)
    {
        var result = await _bookService.UpdateAsync(id, request);

        return result.Match<StatusCodeResult>(
            notfound => NotFound(),
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
            notfound => NotFound(),
            successMessage => NoContent()
        );
    }
}