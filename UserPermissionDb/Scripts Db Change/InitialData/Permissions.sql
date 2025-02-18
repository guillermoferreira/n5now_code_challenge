MERGE Permissions p
USING
(
	SELECT *
	FROM
	(
		VALUES
		(1, 'Guillermo', 'Ferreira', 3, '20250217')
	) r (Id, EmployeeForename, EmployeeSurname, PermissionTypeId, PermissionDate)
) rr ON (rr.Id = p.Id)
WHEN MATCHED THEN UPDATE SET
	p.EmployeeForename = rr.EmployeeForename,
	p.EmployeeSurname = rr.EmployeeSurname,
	p.PermissionTypeId = rr.PermissionTypeId,
	p.PermissionDate = rr.PermissionDate
WHEN NOT MATCHED BY TARGET THEN INSERT
	(EmployeeForename, EmployeeSurname, PermissionTypeId, PermissionDate) 
	VALUES (rr.EmployeeForename, rr.EmployeeSurname, rr.PermissionTypeId, rr.PermissionDate);