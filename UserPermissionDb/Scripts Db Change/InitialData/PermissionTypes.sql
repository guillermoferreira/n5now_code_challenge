MERGE PermissionTypes p
USING
(
	SELECT *
	FROM
	(
		VALUES
		(1, 'Read'),
		(2, 'Read/Write'),
		(3, 'Admin')
	) r (Id, Description)
) rr ON (rr.Id = p.Id)
WHEN MATCHED THEN UPDATE SET
	p.Description = rr.Description
WHEN NOT MATCHED BY TARGET THEN INSERT
	(Description) VALUES (rr.Description);