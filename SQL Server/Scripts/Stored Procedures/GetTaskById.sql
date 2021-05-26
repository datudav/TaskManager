CREATE PROCEDURE [dbo].[GetTaskById]
(	
	@taskId bigint
)
AS
	SELECT
		TaskId,
		UserId,
		ListId,
		Name,
		Description,
		Rank,
		CreatedDate,
		ModifiedDate
	FROM  [dbo].[Task] WITH(NOLOCK)
	WHERE TaskId = @taskId;
GO
