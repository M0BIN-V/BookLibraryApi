using BookLibraryApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Common.Extensions;

public static class WebApplicationExtensions
{
    public static async Task EnsureMigrationsApplied<TDbContext>(this WebApplication app)
        where TDbContext : DbContext
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any()) await dbContext.Database.MigrateAsync();
    }

    public static async Task AddFakeData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!await dbContext.Books.AnyAsync())
            await dbContext.Books.AddRangeAsync(Fakers.GenerateBooks(20));

        if (!await dbContext.Users.AnyAsync())
            await dbContext.Users.AddRangeAsync(Fakers.GenerateUsers(5));
        
        await dbContext.SaveChangesAsync();
    }
}