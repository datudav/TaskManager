ALTER PROCEDURE [dbo].[GetTask]
(	
	@userId bigint,
	@listId bigint,
	@createdDateStart datetime2(3),
	@createdDateEnd datetime2(3),
	@keyword varchar(64) = null,
	@descendingOrder bit = 0
)
AS
	DECLARE @sql      nvarchar(MAX),
			@whereSql nvarchar(1000),
			@params   nvarchar(1000);

	SET @params = N'
		@userId bigint,
		@listId bigint,
		@createdDateStart datetime2(3),
		@createdDateEnd datetime2(3),
		@keyword varchar(64)';

	SET @sql = N'
	SELECT
		TaskId,
		UserId,
		ListId,
		Name,
		Description,
		Rank,
		CreatedDate,
		ModifiedDate
	FROM  [dbo].[Task] WITH(NOLOCK)';

	SET @whereSql = N'
		WHERE UserId = @userId 
		AND ListId = @listId
		AND CreatedDate BETWEEN (@createdDateStart) AND (@createdDateEnd)';

	IF (@keyword IS NOT NULL AND LEN(@keyword) > 0)
	BEGIN
		SET @keyword = '%' + @keyword + '%';
		SET @whereSql = @whereSql + N'
		AND Name LIKE @keyword';
	END

	IF (@descendingOrder = 0)
		SET @whereSql = @whereSql + N'
		ORDER BY [Rank] ASC
		';
	ELSE
		SET @whereSql = @whereSql + N'
		ORDER BY [Rank] DESC
		';

	SET @sql = N'SET NOCOUNT ON;
		' + @sql + @whereSql;

	EXEC sp_executesql @sql, @params, 
				@userId = @userId,
				@listId = @listId,
				@createdDateStart = @createdDateStart,
				@createdDateEnd = @createdDateEnd,
				@keyword = @keyword;
GO
