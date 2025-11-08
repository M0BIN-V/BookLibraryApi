namespace Api.Dtos;

public record BorrowResult(GetBookResult Book, DateTimeOffset BorrowDate, GetUserResult Borrower);