CREATE VIEW [dbo].UnitBundleView
	AS 
		SELECT Units.SerialNumber, Units.Id AS UnitId, 'Unit' AS TableName FROM Units
		WHERE Units.PartOfBundleId IS NULL
	UNION ALL
		SELECT Bundles.SerialNumber, Bundles.Id AS BundleId, 'Bundle' AS TableName FROM Bundles