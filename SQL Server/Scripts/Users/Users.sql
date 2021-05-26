
-- Development
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'Development' AND type = 'S')
BEGIN
	CREATE LOGIN [Development] WITH PASSWORD = 'Devel0pment123!';
END

IF NOT EXISTS (SELECT * FROM dbo.sysusers WHERE name = N'Development') 
BEGIN
	CREATE USER [Development] FOR LOGIN [Development];
END


-- Production
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'Production' AND type = 'S')
BEGIN
	CREATE LOGIN [Production] WITH PASSWORD = 'Pr0duction123!';
END

IF NOT EXISTS (SELECT * FROM dbo.sysusers WHERE name = N'Production') 
BEGIN
	CREATE USER [Production] FOR LOGIN [Production];
END
GO
