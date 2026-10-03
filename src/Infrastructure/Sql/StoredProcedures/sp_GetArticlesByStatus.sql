CREATE OR ALTER PROCEDURE sp_GetArticlesByStatus
    @Status NVARCHAR(100)
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
GO