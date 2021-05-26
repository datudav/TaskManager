CREATE PROCEDURE [dbo].[InsertTask]
(
	@userId bigint,
	@listId bigint,
	@name nvarchar(128),
	@description nvarchar(256) = null,
	@createdDate datetime2,
	@modifiedDate datetime2 
)
AS

BEGIN TRY
	BEGIN TRANSACTION;
		DECLARE @latestRank int = 0;

		IF EXISTS (SELECT TOP 1 1 FROM [dbo].[ToDoList] WHERE ListId = @listId AND UserId = @userId)
		BEGIN	
			SELECT @latestRank = ISNULL(MAX([Rank]), 0)
			FROM [dbo].[Task]
			WHERE ListId = @listId
			AND UserId = @userId;
		END

		IF (@latestRank IS NULL)
			RAISERROR ('The list does not exist.', 16, 1);

		INSERT INTO [dbo].[Task]
		(
			[UserId],
			[ListId],
			[Name],
			[Description],
			[Rank],
			[CreatedDate],
			[ModifiedDate]
		)
		OUTPUT
			INSERTED.[TaskId],
			INSERTED.[UserId],
			INSERTED.[ListId],
			INSERTED.[Name],
			INSERTED.[Description],
			INSERTED.[Rank],
			INSERTED.[CreatedDate],
			INSERTED.[ModifiedDate]
		VALUES
		(
			@userId,
			@listId,
			@name,
			@description,
			@latestRank + 1,
			@createdDate,
			@modifiedDate
		)
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
END CATCH