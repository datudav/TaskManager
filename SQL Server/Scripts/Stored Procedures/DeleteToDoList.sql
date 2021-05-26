CREATE PROCEDURE [dbo].[DeleteToDoList]
(
	@listId bigint
)
AS

BEGIN TRY
	BEGIN TRAN	
		DELETE 
		FROM [dbo].[Task] 
		WHERE ListId = @listId;

		DELETE 
		FROM [dbo].[ToDoList] 
		WHERE ListId = @listId; 

	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK
END CATCH
GO