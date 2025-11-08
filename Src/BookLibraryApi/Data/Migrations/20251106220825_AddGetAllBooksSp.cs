using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibraryApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetAllBooksSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE GetAllBooks
                @PageNumber INT = 1,
                @PageSize INT = 10
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

                SELECT 
                    b.Id,
                    b.Title,
                    b.Author,
                    b.Genre,
                    b.PublishedYear,
                    b.BorrowedAt,
                    b.BorrowedById
                FROM Books b
                ORDER BY b.Id
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;
            END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllBooks;");
        }
    }
}
