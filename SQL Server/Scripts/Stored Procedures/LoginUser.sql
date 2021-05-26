ALTER PROCEDURE [dbo].[LoginUser]
(
    @username varchar(64),
	@password varchar(64)
)
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @userId bigint,
			@valid bit = 0,
			@utcNow datetime2(3) = GETUTCDATE();

	SELECT @userId = UserID 
		FROM [dbo].[User] 
		WHERE Username = @username;

    IF (@userID IS NOT NULL)
    BEGIN
		IF EXISTS(
			SELECT TOP 1 1
			FROM [dbo].[User] 
			WHERE UserId = @userId 
			AND Password = HASHBYTES('SHA2_512', @password + CAST(Salt as nvarchar(36))))
		BEGIN
			SET @valid = 1;
			EXEC [dbo].[UpdateUser] @userID, null, null, @utcNow;
		END
    END
    
	IF (@valid = 0)
       RAISERROR ('Incorrect credentials', 16, 1);
END
GO