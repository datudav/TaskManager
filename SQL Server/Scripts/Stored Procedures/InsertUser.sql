CREATE PROCEDURE [dbo].[InsertUser]
(
	@username varchar(64),
	@password varchar(64),
	@accountStatus tinyint,
	@lastLoginDate datetime2,
	@createdDate datetime2,
	@modifiedDate datetime2 
)
AS

BEGIN TRY
	BEGIN TRANSACTION;
		DECLARE @salt UNIQUEIDENTIFIER = NEWID();

		INSERT INTO [dbo].[User]
		(
			[Username],
			[Password],
			[Salt],
			[AccountStatus],
			[LastLoginDate],
			[CreatedDate],
			[ModifiedDate]
		)
		OUTPUT
			INSERTED.UserId,
			INSERTED.Username,
			INSERTED.LastLoginDate,
			INSERTED.CreatedDate,
			INSERTED.ModifiedDate
		VALUES
		(
			@username,
			HASHBYTES('SHA2_512', @password + CAST(@salt AS NVARCHAR(36))),
			@salt,
			@accountStatus,
			@lastLoginDate,
			@createdDate,
			@modifiedDate
		)
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
END CATCH