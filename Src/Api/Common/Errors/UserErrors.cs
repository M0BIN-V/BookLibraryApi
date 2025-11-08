namespace Api.Common.Errors;

public record UserNotFoundError() : Error("User not found");