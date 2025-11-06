namespace BookLibraryApi.Common.Errors;

public record BookAlreadyBorrowedError(string Title) : Error($"Book '{Title}' is already borrowed");