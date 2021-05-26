CREATE PROCEDURE [dbo].[GetToDoList]
(	
	@userId bigint,
	@createdDateStart datetime2(3),
	@createdDateEnd datetime2(3),
	@keyword varchar(64) = null
)
AS
	DECLARE @sql      nvarchar(MAX),
			@whereSql nvarchar(1000),
			@params   nvarchar(1000);

	SET @params = N'
		@userId bigint,
		@createdDateStart datetime2(3),
		@createdDateEnd datetime2(3),
		@keyword varchar(64)';

	SET @sql = N'
	SELECT
		ListId,
		UserId,
		Name,
		Description,
		CreatedDate,
		ModifiedDate
	FROM  [dbo].[ToDoList] WITH(NOLOCK)';

	SET @whereSql = N'
		WHERE UserId = @userId 
		AND CreatedDate BETWEEN (@createdDateStart) AND (@createdDateEnd)';

	IF (@keyword IS NOT NULL AND LEN(@keyword) > 0)
	BEGIN
		SET @keyword = '%' + @keyword + '%';
		SET @whereSql = @whereSql + N'
		AND Name LIKE @keyword';
	END

	SET @sql = N'SET NOCOUNT ON;
		' + @sql + @whereSql;

	EXEC sp_executesql @sql, @params, 
				@userId = @userId,
				@createdDateStart = @createdDateStart,
				@createdDateEnd = @createdDateEnd,
				@keyword = @keyword;
GO