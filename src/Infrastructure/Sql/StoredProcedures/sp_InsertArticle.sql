CREATE OR ALTER PROCEDURE sp_InsertArticle
    @Id             UNIQUEIDENTIFIER,
    @Title          NVARCHAR(500),
    @AuthorEmail    NVARCHAR(256),
    @Status         NVARCHAR(100),
    @PublishedAt    DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Article (Id, Title, AuthorEmail, Status, PublishedAt, CreatedAt)
    VALUES (@Id, @Title, @AuthorEmail, @Status, @PublishedAt, GETUTCDATE());
END