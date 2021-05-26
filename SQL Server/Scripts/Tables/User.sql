CREATE TABLE [dbo].[User]
(
	[UserId] bigint identity(1,1) not null,
	[Username] nvarchar(64) not null,
    [Password] binary(64) not null,
	[AccountStatus] tinyint not null,
	[LastLoginDate] datetime2(3) not null,
	[CreatedDate] datetime2(3) not null,
	[ModifiedDate] datetime2(3) not null,
	[Salt] uniqueidentifier not null,
	[FailedAttempts] int not null,
	CONSTRAINT [User_PK] PRIMARY KEY CLUSTERED ([UserId] ASC)
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [User_Username_IDX]
    ON [dbo].[User]([Username] ASC)
    ON [PRIMARY];


GO