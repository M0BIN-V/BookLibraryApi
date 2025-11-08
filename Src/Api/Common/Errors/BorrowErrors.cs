namespace Api.Common.Errors;

public record BookAlreadyBorrowedError(string Title) : Error($"Book '{Title}' is already borrowed")
{
    public string Title { get; init; } = Title;
}

public record BookNotBorrowedByUserError() : Error($"Book is not borrowed by user");
