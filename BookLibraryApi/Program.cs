using BookLibraryApi.Common.Extensions;
using BookLibraryApi.Data;
using BookLibraryApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<AppDbContext>("BookLibraryDb");
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

   await app.EnsureMigrationsApplied<AppDbContext>();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();