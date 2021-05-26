CREATE PROCEDURE [dbo].[GetUser]
(
    @username varchar(64) = null,
	@userId bigint = null
)
AS
BEGIN
    SET NOCOUNT ON;

    IF (@username IS NOT NULL)
		SELECT 
			UserId,
			Username,
			LastLoginDate,
			CreatedDate,
			ModifiedDate
		FROM [dbo].[User]
		WHERE Username = @username;
	ELSE IF (@userId IS NOT NULL)
		SELECT 
			UserId,
			Username,
			LastLoginDate,
			CreatedDate,
			ModifiedDate
		FROM [dbo].[User]
		WHERE UserId = @userId;
END
GO
