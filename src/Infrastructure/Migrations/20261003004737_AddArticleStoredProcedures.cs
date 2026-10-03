using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "Sql", "StoredProcedures");

            var insertArticleSql = File.ReadAllText(
                Path.Combine(basePath, "sp_InsertArticle.sql"));

            var getArticlesByStatusSql = File.ReadAllText(
                Path.Combine(basePath, "sp_GetArticlesByStatus.sql"));

            migrationBuilder.Sql(insertArticleSql);
            migrationBuilder.Sql(getArticlesByStatusSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DROP PROCEDURE IF EXISTS sp_InsertArticle");

            migrationBuilder.Sql(
                "DROP PROCEDURE IF EXISTS sp_GetArticlesByStatus");
        }
    }
}
