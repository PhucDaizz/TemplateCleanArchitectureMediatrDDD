using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGetArticlesByStatusProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = File.ReadAllText(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "Sql",
                    "StoredProcedures",
                    "sp_GetArticlesByStatus.sql"));

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE sp_GetArticlesByStatus
                    @Status INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        Id,
                        Title,
                        AuthorEmail,
                        Status,
                        PublishedAt,
                        CreatedAt,
                        UpdatedAt
                    FROM Article
                    WHERE Status = @Status
                    ORDER BY CreatedAt DESC;
                END
                """);
        }
    }
}
