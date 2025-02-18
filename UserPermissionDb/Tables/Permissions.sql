CREATE TABLE [dbo].[Permissions]
(
	[Id] INT NOT NULL IDENTITY, 
    [EmployeeForename] VARCHAR(50) NOT NULL, 
    [EmployeeSurname] VARCHAR(50) NOT NULL, 
    [PermissionTypeId] INT NOT NULL, 
    [PermissionDate] DATE NOT NULL, 
    CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Permissions_PermissionTypes] FOREIGN KEY (PermissionTypeId) REFERENCES [PermissionTypes](Id)
)
