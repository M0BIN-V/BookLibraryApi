using BookLibraryApi.Common.Extensions;
using BookLibraryApi.Data;
using BookLibraryApi.Middlewares;
using BookLibraryApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<AppDbContext>("BookLibraryDb");
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<RequestLoggingMiddleware>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await app.EnsureMigrationsApplied<AppDbContext>();
    await app.AddFakeData();
}

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();