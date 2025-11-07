using BookLibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibraryApi.Data.Configurations;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).HasMaxLength(50);
        builder.Property(b => b.Author).HasMaxLength(50);
        builder.Property(b => b.Genre).HasMaxLength(50);

        builder.HasOne(b => b.BorrowedBy)
            .WithMany()
            .HasForeignKey(b => b.BorrowedById)
            .OnDelete(DeleteBehavior.SetNull);
    }
}