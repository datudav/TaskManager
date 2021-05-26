CREATE PROCEDURE [dbo].[UpdateTask]
(
	@listId bigint,
	@taskId bigint,
	@rank int = null,
	@name nvarchar(128) = null,
	@description nvarchar(256) = null,
	@modifiedDate datetime2(3) 
)
AS
	IF (@rank is not null OR @rank > 0)
	BEGIN
		DECLARE @currentRank int,
				@maxRank int;

		SELECT @maxRank = MAX([Rank])
		FROM [dbo].[Task] WITH (UPDLOCK)
		WHERE ListId = @listId;

		IF (@maxRank IS NOT NULL AND @rank > @maxRank)
		BEGIN
			RAISERROR ('The rank value exceeds the maximum possible value.', 16, 1);
			return;
		END 

		SELECT @currentRank = [Rank]
		FROM [dbo].[Task] WITH (UPDLOCK)
		WHERE TaskId = @taskId;

		CREATE TABLE #Affected
		(
			TaskId	bigint  not null,
			NewRank int		not null
		);

		IF (@currentRank > @rank)
		-- bottom up: 3 to 1
		BEGIN
			INSERT INTO #Affected
			(
				TaskId,
				NewRank
			)
			SELECT 
				TaskId,
				[Rank] + 1
			FROM [dbo].[Task]
			WHERE ListId = @listId
			AND [Rank] >= @rank
			AND [Rank] < @currentRank
			AND TaskId <> @taskId

			UNION

			SELECT
				@taskId,
				@rank
		
		END
		ELSE IF (@currentRank < @rank)
		-- top bottom: 1 to 3
		BEGIN
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
			AND [Rank] > @currentRank
			AND [Rank] <= @rank
			AND TaskId <> @taskId

			UNION

			SELECT
				@taskId,
				@rank
		END

		UPDATE T SET
			[Rank] = A.NewRank,
			[ModifiedDate] = @modifiedDate
		FROM [dbo].[Task] T 
		INNER JOIN #Affected A ON T.TaskId = A.TaskId

		UPDATE [dbo].[Task]
		SET 
			[Name] = CASE WHEN @name IS NULL OR LEN(@name) = 0 THEN [Name] ELSE @name END,
			[Description] = CASE WHEN @description IS NULL OR LEN(@description) = 0 THEN [Description] ELSE @description END,
			[ModifiedDate] = @modifiedDate
		WHERE TaskId = @taskId;
	END
GO