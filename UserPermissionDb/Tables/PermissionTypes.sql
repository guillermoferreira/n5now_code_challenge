CREATE TABLE [dbo].[PermissionTypes]
(
	[Id] INT NOT NULL IDENTITY , 
    [Description] VARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_PermissionTypes] PRIMARY KEY ([Id])
)
