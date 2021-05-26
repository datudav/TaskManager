CREATE PROCEDURE [dbo].[DeleteTask]
(
	@listId bigint,
	@taskId bigint,
	@modifiedDate datetime2(3)
)
AS

BEGIN TRY
	BEGIN TRAN

		DECLARE @currentRank int;

		SELECT @currentRank = [Rank]
		FROM [dbo].[Task] WITH (UPDLOCK)
		WHERE TaskId = @taskId;

		CREATE TABLE #Affected
		(
			TaskId	bigint  not null,
			NewRank int		not null
		);

		INSERT INTO #Affected
		(
			TaskId,
			NewRank
		)
		SELECT 
			TaskId,
			[Rank] - 1
		FROM [dbo].[Task]
		WHERE ListId = @listId
		AND [Rank] > @currentRank;
	
		IF EXISTS(SELECT TOP 1 1 FROM #Affected)
		BEGIN
			UPDATE T SET
				[Rank] = A.NewRank,
				[ModifiedDate] = @modifiedDate
			FROM [dbo].[Task] T 
			INNER JOIN #Affected A ON T.TaskId = A.TaskId
		END

		DELETE 
		FROM [dbo].[Task] 
		WHERE TaskId = @taskId;

	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK
END CATCH
GO