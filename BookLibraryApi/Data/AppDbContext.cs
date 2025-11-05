using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) { }