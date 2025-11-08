namespace BookLibraryApi.Common.Errors;

public record BookNotFoundError() : Error("Book not found");