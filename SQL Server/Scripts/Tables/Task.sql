CREATE TABLE [dbo].[Task]
(
	[TaskId] bigint identity(1,1) not null,
	[UserId] bigint not null,
	[ListId] bigint not null,
	[Name] nvarchar(128) not null,
	[Description] nvarchar(256) null,
	[Rank] int not null,
	[CreatedDate] datetime2(3) not null,
	[ModifiedDate] datetime2(3) not null,
	CONSTRAINT [Task_PK] PRIMARY KEY CLUSTERED ([TaskId] ASC),
	CONSTRAINT [Task_User_FK] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
	CONSTRAINT [Task_ToDoList_FK] FOREIGN KEY ([ListId]) REFERENCES [dbo].[ToDoList] ([ListId])
);
GO

