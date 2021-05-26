CREATE PROCEDURE [dbo].[UpdateUser]
(
	@userId bigint,
	@password nvarchar(64) = null,
	@accountStatus tinyint = null,
	@lastLoginDate datetime2 = null
)
AS

BEGIN TRY
	BEGIN TRANSACTION;
		UPDATE U
			SET
				U.AccountStatus = ISNULL(@accountStatus, U.AccountStatus),
				U.LastLoginDate = ISNULL(@lastLoginDate, U.LastLoginDate),
				U.ModifiedDate = GETUTCDATE()
		FROM [dbo].[User] U
		WHERE U.UserId = @userId

		IF (@password IS NOT NULL)
		BEGIN
			DECLARE @salt UNIQUEIDENTIFIER = NEWID();

			UPDATE U
			SET
				U.Password = HASHBYTES('SHA2_512', @password + CAST(@salt AS NVARCHAR(36))),
				U.Salt = @salt
			FROM [dbo].[User] U
			WHERE U.UserId = @userId;
		END
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
END CATCH