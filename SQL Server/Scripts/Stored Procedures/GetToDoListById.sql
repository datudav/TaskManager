CREATE PROCEDURE [dbo].[GetToDoListById]
(	
	@listId bigint
)
AS
	SELECT
		ListId,
		UserId,
		Name,
		Description,
		CreatedDate,
		ModifiedDate
	FROM  [dbo].[ToDoList] WITH(NOLOCK)
	WHERE ListId = @listId;
GO