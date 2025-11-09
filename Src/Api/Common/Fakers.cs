using Api.Models;
using Bogus;

namespace Api.Common;

public static class Fakers
{
    static readonly Faker<User> UserFaker = new();
    static readonly Faker<Book> BookFaker = new();

    public static List<User> GenerateUsers(int count)
    {
        UserFaker
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Name, f => f.Name.FullName());

        return UserFaker.Generate(count);
    }

    public static List<Book> GenerateBooks(int count)
    {
        BookFaker
            .RuleFor(b => b.Author, f => f.Name.FullName())
            .RuleFor(b => b.Title, f => f.Lorem.Word())
            .RuleFor(b => b.Genre, f => f.Lorem.Word())
            .RuleFor(b => b.PublishedYear,
                f => f.Date.Between(DateTime.UtcNow.AddYears(-100), DateTime.UtcNow).Year);
        
        return BookFaker.Generate(count);
    }
}