CREATE TABLE [dbo].[ToDoList]
(
	[ListId] bigint identity(1,1) not null,
	[UserId] bigint not null,
	[Name] nvarchar(128) not null,
    [Description] nvarchar(256) not null,
	[CreatedDate] datetime2(3) not null,
	[ModifiedDate] datetime2(3) not null,
	CONSTRAINT [ToDoList_PK] PRIMARY KEY CLUSTERED ([ListId] ASC),
	CONSTRAINT [ToDoList_User_FK] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);
GO
