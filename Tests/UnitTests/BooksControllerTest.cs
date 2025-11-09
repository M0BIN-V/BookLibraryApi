using Api.Controllers;
using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using static NSubstitute.Arg;
using static NSubstitute.Substitute;

namespace UnitTests;

public class BooksControllerTest
{
    readonly IBookService _bookService = For<IBookService>();
    readonly IBorrowService _borrowService = For<IBorrowService>();
    readonly ILogger<BooksController> _logger = For<ILogger<BooksController>>();

    [Fact]
    public async Task CreateBook_ShouldReturn201()
    {
        //Arrange
        var request = new AddBookRequest
        {
            Title = "this is title",
            Author = "this is author",
            Genre = "this is genre",
            PublishedYear = 1990
        };
        _bookService.AddAsync(Any<AddBookRequest>())
            .Returns(new GetBookResult
            {
                Id = 10,
                Title = request.Title,
                Author = request.Author,
                Genre = request.Genre,
                PublishedYear = request.PublishedYear
            });
        var controller = new BooksController(_bookService, _borrowService, _logger);

        //Act
        var result = await controller.CreateAsync(request);

        //Assert
        result.ShouldBeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task GetBook_WhenBookNotFound_ShouldReturn404()
    {
        //Arrange
        _bookService.GetByIdAsync(999)
            .Returns(Task.FromResult<GetBookResult?>(null));
        const int requestBookId = 999;
        var controller = new BooksController(_bookService, _borrowService, _logger);

        //Act
        var result = await controller.Get(requestBookId);

        //Assert
        result.ShouldBeOfType<NotFoundResult>();
    }
}