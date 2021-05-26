CREATE PROCEDURE [dbo].[InsertToDoList]
(
	@userId bigint,
	@name nvarchar(128),
	@description nvarchar(256) = null,
	@createdDate datetime2,
	@modifiedDate datetime2 
)
AS

BEGIN TRY
	BEGIN TRANSACTION;
		INSERT INTO [dbo].[ToDoList]
		(
			[UserId],
			[Name],
			[Description],
			[CreatedDate],
			[ModifiedDate]
		)
		OUTPUT
			INSERTED.ListId,
			INSERTED.UserId,
			INSERTED.Name,
			INSERTED.Description,
			INSERTED.CreatedDate,
			INSERTED.ModifiedDate
		VALUES
		(
			@userId,
			@name,
			@description,
			@createdDate,
			@modifiedDate
		)
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
END CATCH