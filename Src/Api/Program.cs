using Api.Common.Extensions;
using Api.Data;
using Api.Middlewares;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookLibraryDb")));
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        var url = builder.Configuration["OpenApi:Url"];

        if (!string.IsNullOrWhiteSpace(url))
            document.Servers = new List<OpenApiServer>
            {
                new() { Url = url }
            };

        return Task.CompletedTask;
    });
});
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

app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();