CREATE PROCEDURE [dbo].[UpdateToDoList]
(
	@listId bigint,
	@name nvarchar(64) = null,
	@description nvarchar(64) = null,
	@modifiedDate datetime2 = null
)
AS
BEGIN TRY
	BEGIN TRANSACTION;
		UPDATE [dbo].[ToDoList]
		SET 
			[Name] = CASE WHEN @name IS NULL OR LEN(@name) = 0 THEN [Name] ELSE @name END,
			[Description] = CASE WHEN @description IS NULL OR LEN(@description) = 0 THEN [Description] ELSE @description END,
			[ModifiedDate] = @modifiedDate
		WHERE ListId = @listId;
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
END CATCH